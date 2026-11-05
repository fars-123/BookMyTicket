using BookMyTicket.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookMyTicket.Controllers
{
    public class ScreenController : Controller
    {
        ScreenDB db = new ScreenDB();  
        public IActionResult Screen_Insert_Pageload()
        {
            Screen obj = new Screen();

            obj.Movie_List =db.Movie_Select();

            return View(obj);
        }
        
        public IActionResult Screen_Insert_Button_Click(Screen obj)
        {
            if (ModelState.IsValid)
            {
                string role = Convert.ToString(TempData["Role"]);

                if (role == "Theatre Owner")
                {
                    int userId = Convert.ToInt32(TempData["User_Id"]);
                    obj.User_Id = userId;

                    List<Movie> movieList = db.Movie_Select();

                    foreach (var item in movieList)
                    {
                        if (item.Movie_Id == obj.Movie_Id)
                        {
                            obj.Movie_Id = item.Movie_Id;
                            obj.Movie_Name = item.Movie_Name;
                        }
                    }

                    string result = db.Screen_Insert(obj);



                    TempData["msg"] = result;


                    return RedirectToAction("Screen_Insert_Pageload",obj);

                }


            }

            return View("Screen_Insert_Pageload");

        }
        public IActionResult View_Screen_Button_Click()
        {
            int userId = Convert.ToInt32(TempData["User_Id"]);

            TempData["User_Id"] = userId;

            List<Screen> list = db.Screen_View(userId);

            return View(list);
        }

        public IActionResult Edit_Screen(int screen_id)
        {
            List<Screen> list = db.Edit_Screen(screen_id);

            Screen obj = list[0];

            obj.Movie_List = db.Movie_Select();

            return View(obj);
        }
        public IActionResult Update_Screen(Screen obj)
        {
            List<Movie> movieList = db.Movie_Select();

            foreach (var item in movieList)
            {
                if (item.Movie_Id == obj.Movie_Id)
                {
                    obj.Movie_Name = item.Movie_Name;
                }
            }

            string result = db.Update_Screen(obj);

            TempData["msg"] = result;
            TempData["User_Id"] = obj.User_Id;

            return RedirectToAction("View_Screen_Button_Click");
        }
        public IActionResult Delete_Screen(int screen_id)
        {
            int count = db.Check_Show_Booking(screen_id);

            if (count > 0)
            {
                TempData["msg"] = "Cannot delete this show because bookings already exist.";
            }
            else
            {
                string result = db.Delete_Screen(screen_id);
                TempData["msg"] = result;
            }

            return RedirectToAction("View_Screen_Button_Click");
        }
        public IActionResult Show_IsEnable_Update(int Screen_Id, bool IsEnable)
        {
            int User_Id = Convert.ToInt32(TempData["User_Id"]);

            db.Show_IsEnable_Update(Screen_Id, User_Id, IsEnable);

            TempData["User_Id"] = User_Id;

            if (IsEnable == true)
            {
                TempData["msg"] = "Show enabled successfully.";
            }
            else
            {
                TempData["msg"] = "Show disabled successfully.";
            }

            return RedirectToAction("View_Screen_Button_Click");
        }

    }
}
