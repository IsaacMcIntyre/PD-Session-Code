using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FileUploader.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace FileUploader.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
           List<string> hobbies = new List<string> { "Bank Robbing", "Sleep", "Breathing"};
           return View(hobbies);
        }

        //[HttpPost]
        //public IActionResult IndexPost()
        //{
        //    string hobby = this.Request.Form["hobby"];
        //    List<string> hobbies = new List<string> { "Bank Robbing", "Sleep", "Breathing", hobby };

        //    return View("Index", hobbies);
        //}

        [HttpPost]
        public async Task<IActionResult> IndexPost(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            return Ok(new { count = files.Count, size, filePath });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
