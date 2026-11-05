using BookMyTicket.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace BookMyTicket.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IConfiguration _configuration;

        public CustomerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        MovieDB db = new MovieDB();
        UserDB dbj = new UserDB();
        ScreenDB ob = new ScreenDB();
        BookingDB bookingDB = new BookingDB();

        public IActionResult Customer_Pageload()
        {
            List<Movie> list = db.Movie_View();

            return View(list);
        }

        public IActionResult View_Theatre_with_Shows(int movie_id)
        {
            List<Show> list = dbj.View_TheatreShows(movie_id);

            return View(list);
        }

        public IActionResult View_Seats(int screen_id)
        {
            List<Screen> list = ob.Get_Screen(screen_id);

            if (list.Count == 0)
            {
                return NotFound("This show is disabled or unavailable.");
            }

            list[0].UnavailableSeats = bookingDB.Get_Unavailable_Seats(
                screen_id,
                list[0].ShowDate,
                list[0].StartTime);

            return View(list);
        }

        [HttpPost]
        public IActionResult Continue_Booking_seats(
            List<string> seats, int Screen_Id)
        {
            if (seats == null || seats.Count == 0)
            {
                return RedirectToAction(
                    "View_Seats",
                    new { screen_id = Screen_Id });
            }

            List<Screen> screenList = ob.Get_Screen(Screen_Id);

            if (screenList.Count == 0)
            {
                return NotFound("This show is disabled or unavailable.");
            }

            Booking Bill = new Booking();

            Bill.Screen_Id = Screen_Id;
            Bill.Seats = seats;
            Bill.SeatCount = seats.Count;
            Bill.ShowDate = screenList[0].ShowDate;
            Bill.StartTime = screenList[0].StartTime;
            Bill.TicketPrice = ob.Get_TicketPrice(Screen_Id);
            Bill.TotalAmount = Bill.SeatCount * Bill.TicketPrice;

            return View(Bill);
        }

        [HttpPost]
        public IActionResult Payment(int Screen_Id, List<string> Seats)
        {
            if (Seats == null || Seats.Count == 0)
            {
                return RedirectToAction(
                    "View_Seats",
                    new { screen_id = Screen_Id });
            }

            if (TempData.Peek("User_Id") == null)
            {
                return RedirectToAction("Login_Pageload", "Login");
            }

            List<Screen> screenList = ob.Get_Screen(Screen_Id);

            if (screenList.Count == 0)
            {
                return NotFound(
                    "This show is disabled or unavailable. Booking is not allowed.");
            }

            Booking Bill = new Booking();

            Bill.User_Id = Convert.ToInt32(TempData.Peek("User_Id"));
            Bill.Screen_Id = Screen_Id;
            Bill.Seats = Seats;
            Bill.SeatCount = Seats.Count;

            // Get the actual show date and time
            Bill.ShowDate = screenList[0].ShowDate;
            Bill.StartTime = screenList[0].StartTime;

            Bill.TicketPrice = ob.Get_TicketPrice(Screen_Id);
            Bill.TotalAmount = Bill.SeatCount * Bill.TicketPrice;
            Bill.PaymentStatus = "Pending";

            string keyId = _configuration["Razorpay:KeyId"];
            string keySecret = _configuration["Razorpay:KeySecret"];

            if (string.IsNullOrEmpty(keyId) ||
                string.IsNullOrEmpty(keySecret))
            {
                return StatusCode(
                    500, "Razorpay configuration is missing.");
            }

            RazorpayClient client =
                new RazorpayClient(keyId, keySecret);

            Dictionary<string, object> options =
                new Dictionary<string, object>();

            options.Add(
                "amount",
                Convert.ToInt32(Bill.TotalAmount * 100));

            options.Add("currency", "INR");
            options.Add("receipt", Guid.NewGuid().ToString());

            // Create Razorpay order
            Order order = client.Order.Create(options);

            Bill.RazorpayKeyId = keyId;
            Bill.RazorpayOrderId = order["id"].ToString();

            // Save booking with Pending status and Razorpay Order ID
            bookingDB.Insert_Booking(Bill);

            // Open the payment page
            return View(Bill);
        }

        public IActionResult VerifyPayment(
            string paymentId,
            string orderId,
            string signature)
        {
            if (string.IsNullOrEmpty(paymentId) ||
                string.IsNullOrEmpty(orderId) ||
                string.IsNullOrEmpty(signature))
            {
                return Content("Payment details are missing.");
            }

            string keySecret =
                _configuration["Razorpay:KeySecret"];

            if (string.IsNullOrEmpty(keySecret))
            {
                return StatusCode(
                    500, "Razorpay configuration is missing.");
            }

            string message = orderId + "|" + paymentId;

            using (var hmac = new HMACSHA256(
                Encoding.UTF8.GetBytes(keySecret)))
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                byte[] hash = hmac.ComputeHash(data);

                string expectedSignature =
                    Convert.ToHexString(hash).ToLowerInvariant();

                byte[] expectedBytes =
                    Encoding.UTF8.GetBytes(expectedSignature);

                byte[] actualBytes =
                    Encoding.UTF8.GetBytes(signature.ToLowerInvariant());

                if (expectedBytes.Length != actualBytes.Length ||
                    !CryptographicOperations.FixedTimeEquals(
                        expectedBytes, actualBytes))
                {
                    return Content("Payment verification failed.");
                }
            }

            string keyId = _configuration["Razorpay:KeyId"];

            RazorpayClient client =
                new RazorpayClient(keyId, keySecret);

            Payment payment = client.Payment.Fetch(paymentId);

            string status = payment["status"].ToString();
            string paymentOrderId = payment["order_id"].ToString();

            if (status == "captured" &&
                paymentOrderId == orderId)
            {
                bookingDB.Update_Payment(orderId, paymentId);

                return View();
            }

            return Content("Payment is not captured yet.");
        }
    }
}