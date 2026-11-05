using BookMyTicket.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookMyTicket.Controllers
{
    public class ChangePasswordController : Controller
    {
        UserDB db = new UserDB();

        // opens when the user clicks the link in the email
        public IActionResult ChangePassword_Pageload(string token)
        {
            string? email = db.GetEmailByToken(token);

            if (email == null)
            {
                TempData["msgfp"] = "This link is not valid.";
                return RedirectToAction("ForgotPassword_Pageload", "ForgotPassword");
            }

            return View(new ResetPassword_Model { Token = token, Email = email });
        }

        // runs when the user clicks "Submit"
        [HttpPost]
        public IActionResult ChangePassword_Click(ResetPassword_Model m)
        {
            if (!ModelState.IsValid)
                return View("ChangePassword_Pageload", m);

            try
            {
                if (db.ResetPassword(m.Token, m.NewPassword))
                {
                    TempData["msglog"] = "Password changed. Please login.";
                    return RedirectToAction("Login_Pageload", "Login");
                }

                TempData["msgfp"] = "This link is not valid.";
            }
            catch (Exception ex)
            {
                TempData["msgfp"] = ex.Message;
            }

            return View("ChangePassword_Pageload", m);
        }
    }
}