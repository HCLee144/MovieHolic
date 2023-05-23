using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjMovieHolic.Models;
using prjMovieHolic.ViewModels;

namespace prjMovieHolic.Controllers
{
    public class movieBackController : BackSuperController
    {
        private readonly MovieContext _db;
        private readonly IWebHostEnvironment _enviro;

        public movieBackController(MovieContext context, IWebHostEnvironment p)
        {
            _db = context;
            _enviro = p;
        }

        public IActionResult MovieView(int? messageCode)
        {
            if(messageCode !=null)
                ViewBag.MessageCode = messageCode;
            return View();
        }

        public IActionResult queryMovieByName(string movieName)
        {
            var movie = _db.TMovies.Where(m => m.FNameCht == movieName).FirstOrDefault();
            return Json(movie);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CMovieBackViewModel edition)
        {
            TMovie movie = _db.TMovies.Where(m => m.FId == edition.FId).FirstOrDefault();
            if (movie != null)
            {
                if (edition.FNameCht != null)
                    movie.FNameCht = edition.FNameCht;
                if (edition.FNameEng != null)
                    movie.FNameEng = edition.FNameEng;
                movie.FScheduleStart = edition.FScheduleStart;
                movie.FScheduleEnd = edition.FScheduleEnd;
                if (edition.FShowLength != null)
                    movie.FShowLength = edition.FShowLength;
                if (edition.FInteroduce != null)
                    movie.FInteroduce = edition.FInteroduce;
                if (edition.FTrailerLink != null)
                    movie.FTrailerLink = edition.FTrailerLink;
                if (edition.FPrice != null)
                    movie.FPrice = edition.FPrice;
                if(edition.image != null)
                {
                    string photoName = $"/images/moviePosters/{movie.FId}/"+Guid.NewGuid().ToString() + ".jpg";
                    string path = _enviro.WebRootPath +photoName;
                    edition.image.CopyTo(new FileStream(path, FileMode.Create));
                    movie.FPosterPath = photoName;
                }
                _db.SaveChanges();
            }
            return RedirectToAction("MovieView");
        }
    }
}
