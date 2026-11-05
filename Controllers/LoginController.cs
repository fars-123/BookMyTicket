using BookMyTicket.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookMyTicket.Controllers
{
    public class LoginController : Controller
    {
        UserDB db = new UserDB();
        public IActionResult Login_Pageload()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login_Button_Click(User_Register obj)
        {
            try
            {
                var s = db.UserLogin(obj);

                if (s.User_Id != 0)
                {
                    if (s.IsActive == false)
                    {
                        TempData["msglog"] = "Your account is inactive.";
                        return View("Login_Pageload");
                    }

                    if (s.Role == "Theatre Owner" && s.IsApproved == false)
                    {
                        TempData["msglog"] = "Your account is waiting for Admin approval.";
                        return View("Login_Pageload");
                    }

                    TempData["User_Id"] = s.User_Id;
                    TempData["Role"] = s.Role;

                    if (s.Role == "Admin")
                    {
                        return RedirectToAction("Admin_Pageload", "Admin");
                    }
                    else if (s.Role == "Theatre Owner")
                    {
                        return RedirectToAction("TheatreOwner_Pageload", "Theatre");
                    }
                    else if (s.Role == "User")
                    {
                        return RedirectToAction("Customer_Pageload", "Customer");
                    }
                }
                else
                {
                    TempData["msglog"] = "Invalid Login";
                }

                return View("Login_Pageload");
            }
            catch (Exception ex)
            {
                TempData["msglog"] = ex.Message;
                return View("Login_Pageload");
            }
        }

      
    }
}
