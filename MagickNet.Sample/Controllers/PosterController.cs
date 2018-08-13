using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageMagick;
using MagickNet.Sample.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MagickNet.Sample.Controllers {
    public class PosterController : Controller {
        string webrootPath;
        string contentRootPath;
        string imagePath;
        // GET: /<controller>/
        public PosterController(IHostingEnvironment hostingEnvironment)
        {
            webrootPath = hostingEnvironment.WebRootPath;
            contentRootPath = hostingEnvironment.ContentRootPath;
            imagePath = Path.Combine(webrootPath, "images");
        }
        public IActionResult Index()
        {
            PosterModel model;
          
                model = new PosterModel();
                model.ImageSrc = "/images/large_Event_Poster_Pasta.png";
          

            return View(model);
        }
        [HttpPost]
        public IActionResult Index(PosterModel model)
        {
            string originalImage = Path.Combine(webrootPath, "images", "large_Event_Poster_Pasta.png");
            string newFilePath = Path.Combine(webrootPath, "images", "poster_with_data.png");
            string font = Path.Combine(webrootPath, "fonts", "TGSharpSans-Medium.ttf");
            string extraBoldFont = Path.Combine(webrootPath, "fonts", "TGSharpSans-Semibold.ttf");
            string thinFont = Path.Combine(webrootPath, "fonts", "TGSharpSans-Thin.ttf");
          
            using (MagickImage image = new MagickImage(originalImage)) {
                
                // image.Draw(drawables);
                var readSettings = new MagickReadSettings {
                    FillColor = MagickColors.Black,
                    BackgroundColor = MagickColors.Transparent,
                   // TextGravity = Gravity.Center,
                    Width = 400,                   
                    StrokeColor = MagickColors.Black,
                    Font = extraBoldFont,
                    FontPointsize = 36,             

                };
                using (var caption = new MagickImage($"caption: {model.EventName.Replace('~','\n').ToUpper()}", readSettings)) {

                    image.Composite(caption, 100, 500, CompositeOperator.Over);
                }
                readSettings.Font = font;
                using (var caption = new MagickImage($"caption: {model.EventDate.Replace('~', '\n').ToUpper()}", readSettings)) {

                    image.Composite(caption, 100, 600, CompositeOperator.Over);
                }
                readSettings.FontPointsize = 18;
                using (var caption = new MagickImage($"caption: {model.Location.Replace('~', '\n').ToUpper()}", readSettings)) {

                    image.Composite(caption, 100, 650, CompositeOperator.Over);
                }

                DrawableLine line = new DrawableLine(100, 700, 600, 700);
                image.Draw(line);
                readSettings.FontPointsize = 14;
                readSettings.Width = 200;
                readSettings.Font = thinFont;
                using (var caption = new MagickImage($"caption: {model.Explanation}", readSettings)) {

                    image.Composite(caption, 100, 725, CompositeOperator.Over);
                }
                using (var caption = new MagickImage($"caption: {model.AdditionalDetails}", readSettings)) {

                    image.Composite(caption, 400, 725, CompositeOperator.Over);
                }

                //var eventNameDrawable = new Drawables()
                //    .Font(@"H:\publicH\Downloads\TenderGreensSharpSans-Extrabold.woff2",FontStyleType.Bold,FontWeight.Bold, FontStretch.Normal)
                //    .FontPointSize(72)

                //    .StrokeColor(MagickColors.Black)
                //    .Text(100,600, model.EventName.ToUpper())

                //    .Draw(image);


                //var drawable = new Drawables()
                //    //.FontPointSize(72)
                //    //.Font(extraBoldFont)
                //    //.StrokeColor(MagickColors.Black)
                //    //.TextAlignment(TextAlignment.Left)                    
                //   // .Text(100, 600, model.EventName.ToUpper())
                //     .FontPointSize(36)
                //    .Font(font)
                //    .StrokeColor(MagickColors.Black)
                //    .TextAlignment(TextAlignment.Left)
                //    .Text(100, 700, model.EventDate.ToUpper())
                //    .Text(100, 800, model.Location.ToUpper())
                //    //.StrokeColor(new MagickColor(0, Quantum.Max, 0))
                //    //.FillColor(MagickColors.SaddleBrown)
                //    //.Ellipse(256, 96, 192, 8, 0, 360)
                //    .Draw(image);
                image.Write(newFilePath);
                model.ImageSrc = "/images/poster_with_data.png";
                //ExifProfile profile = image.GetExifProfile();
                //if (profile == null) {
                //    ViewData["Message"] = "Image does not contain exif information";
                //}
                //else {
                //    string message = "";
                //    foreach (ExifValue value in profile.Values) {
                //        message += $"{value.Tag} ({value.DataType}): {value.ToString()}<br />";
                //    }
                //}
            }

            return View(model);
        }
    }
}
