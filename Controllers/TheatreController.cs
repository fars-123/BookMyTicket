using BookMyTicket.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookMyTicket.Controllers
{
    public class TheatreController : Controller
    {

            ScreenDB db = new ScreenDB();
            BookingDB dbj = new BookingDB();

        public IActionResult TheatreOwner_Pageload()
        {
            int User_Id = Convert.ToInt32(TempData["User_Id"]);

            TempData["User_Id"] = User_Id;

            Screen obj = new Screen();

            obj.TotalScreens = db.Screen_Count(User_Id);
            obj.TotalShows = db.Show_Count(User_Id);
            obj.TotalBookings = dbj.Theatre_Booking_Count(User_Id);
            obj.TotalRevenue = dbj.Theatre_Total_Revenue(User_Id);
            obj.Booking_List = dbj.Theatre_View_Bookings(User_Id);

            return View(obj);
        }
    }
   
}


