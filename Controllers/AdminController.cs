using BookMyTicket.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BookMyTicket.Controllers
{
    public class AdminController : Controller
    {
        UserDB db = new UserDB();
        MovieDB movieDB = new MovieDB();
        BookingDB bookingDB = new BookingDB();

        public IActionResult Admin_Pageload()
        {
            Admin admin = new Admin();

            admin.MovieList = movieDB.Movie_View();
            admin.TotalUsers = db.User_Count();
            admin.TotalMovies = movieDB.Movie_Count();
            admin.ApprovedOwnerCount = db.Approved_TheatreOwner_Count();
            admin.TotalBookings = bookingDB.Booking_Count();

            List<User_Register> pendingList = db.Pending_TheatreOwners();
            admin.PendingOwnerCount = pendingList.Count;

            return View(admin);
        }
        public IActionResult Pending_TheatreOwners()
        {
            List<User_Register> list = db.Pending_TheatreOwners();

            return View(list);
        }

        public IActionResult Approve_TheatreOwner(int User_Id)
        {
            db.Approve_TheatreOwner(User_Id);

            TempData["msg"] = "Theatre Owner approved successfully.";

            return RedirectToAction("Pending_TheatreOwners");
        }

        public IActionResult Movie_IsEnable_Update(int Movie_Id, bool IsEnable)
        {
            movieDB.Movie_IsEnable_Update(Movie_Id, IsEnable);

            return RedirectToAction("Admin_Pageload");
        }
    }
}