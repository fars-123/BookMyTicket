using BookMyTicket.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookMyTicket.Controllers
{
    public class ForgotPasswordController : Controller
    {
        UserDB db = new UserDB();

        public IActionResult ForgotPassword_Pageload()
        {
            return View();
        }

        public IActionResult ForgotPassword_Click(ForgotPassword_Model m)
        {
            try
            {
                string token = Guid.NewGuid().ToString("N");

                if (db.SaveResetToken(m.Email, token))
                {
                    string link = Url.Action("ResetPassword_Pageload", "Password", new { token = token }, Request.Scheme);

                    db.SendEmail(m.Email, "Reset your password",
                        "Click this link to change your password: <a href='" + link + "'>Change Password</a>");
                }

                TempData["msgfp"] = "If this email is registered, a reset link has been sent.";
            }
            catch (Exception ex)
            {
                TempData["msgfp"] = ex.Message;
            }

            return View("ForgotPassword_Pageload");
        }

        
    }
}