using BookMyTicket.Models;
using Microsoft.AspNetCore.Mvc;


namespace BookMyTicket.Controllers
{
    public class RegisterController : Controller
    {
        UserDB db = new UserDB();
        public IActionResult UserRegister_Pageload()
        {
            return View();
        }
        public IActionResult Insert_Click(User_Register objcls)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (objcls.Role == "User"|| objcls.Role == "Admin")
                    {
                        objcls.IsApproved = true;
                        objcls.IsActive = true;
                    }
                    else if (objcls.Role == "Theatre Owner")
                    {
                        objcls.IsApproved = false;
                        objcls.IsActive = true;
                    }                  
                    string response = db.UserRegInsertDB(objcls);
                    TempData["msg"]=response;

                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;

            }
            return View("UserRegister_Pageload",objcls);
        }
    }
}
