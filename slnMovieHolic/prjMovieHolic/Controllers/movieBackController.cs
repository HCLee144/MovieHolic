using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjMovieHolic.Models;

namespace prjMovieHolic.Controllers
{
    public class movieBackController : Controller
    {
        private readonly MovieContext _db;

        public movieBackController(MovieContext context)
        {
            _db = context;
        }

        public IActionResult MovieView()
        {
            return View();
        }

        public IActionResult queryMovieByName(string movieName)
        {
            var movie = _db.TMovies.Where(m => m.FNameCht == movieName).FirstOrDefault();
            return Json(movie);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TMovie edition)
        {
            TMovie movie = _db.TMovies.Where(m => m.FId == edition.FId).FirstOrDefault();
            if(movie != null)
            {
                movie.FNameCht= edition.FNameCht;
                movie.FNameEng= edition.FNameEng;
                movie.FScheduleStart= edition.FScheduleStart;
                movie.FScheduleEnd= edition.FScheduleEnd;
                movie.FShowLength = edition.FShowLength;
                movie.FInteroduce = edition.FInteroduce;
                movie.FTrailerLink= edition.FTrailerLink;
                movie.FPrice = edition.FPrice;
                _db.SaveChanges();
            }
            return RedirectToAction("MovieView");
        }
    }
}
