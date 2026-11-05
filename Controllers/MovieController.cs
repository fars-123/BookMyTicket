using BookMyTicket.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
namespace BookMyTicket.Controllers
{
    public class MovieController : Controller
    {
        MovieDB db = new MovieDB();
        public IActionResult Movie_Pageload()
        {
            return View();
        }

        public IActionResult Movie_Insert_Button_click(Movie obj)
        {
            {
                if (obj.Movie_Image_File != null)
                {
                    string fileName = obj.Movie_Image_File.FileName;

                    string folderPath = Directory.GetCurrentDirectory() + "/wwwroot/MovieImages/";
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string filePath = folderPath + fileName;

                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        obj.Movie_Image_File.CopyTo(fs);
                    }

                    obj.Movie_Image = fileName;
                }

                string result = db.Movie_Insert(obj);

                TempData["msg"] = result;

                return RedirectToAction("Movie_Pageload");
            }

            

        }
       

    }
}
