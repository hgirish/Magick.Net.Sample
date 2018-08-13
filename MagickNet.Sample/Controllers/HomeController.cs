using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MagickNet.Sample.Models;
using ImageMagick;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace MagickNet.Sample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public HomeController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            var rootpath = _hostingEnvironment.ContentRootPath;
            var webRootPath = _hostingEnvironment.WebRootPath;
            ViewData["Message"] = "Your application description page.";
            string filePath = Path.Combine(webRootPath, "images", "imagesharp-logo.png");
            string newFilePath = Path.Combine(webRootPath, "images", "watermark-image.png");
            using (MagickImage image = new MagickImage(filePath))
            {
                // image.Draw(drawables);
                var drawable = new Drawables()
                    .FontPointSize(72)
                    .Font(@"D:\Downloads\Pacifico-Regular.ttf")
                    .StrokeColor(MagickColors.Orange)
                    .TextAlignment(TextAlignment.Center)
                    .Text(256, 64, "Magick.NET")
                    .StrokeColor(new MagickColor(0, Quantum.Max, 0))
                    .FillColor(MagickColors.SaddleBrown)
                    .Ellipse(256, 96, 192, 8, 0, 360)
                    .Draw(image);
                image.Write(newFilePath);
                ExifProfile profile = image.GetExifProfile();
                if (profile == null)
                {
                    ViewData["Message"] = "Image does not contain exif information";
                }
                else
                {
                    string message = "";
                    foreach (ExifValue value in profile.Values)
                    {
                        message += $"{value.Tag} ({value.DataType}): {value.ToString()}<br />";
                    }
                }
            }

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
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
