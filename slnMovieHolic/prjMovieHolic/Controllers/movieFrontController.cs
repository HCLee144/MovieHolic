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
    public class movieFrontController : Controller
    {
        private readonly MovieContext _context;

        public movieFrontController(MovieContext context)
        {
            _context = context;
        }

        // GET: movieFront
        public async Task<IActionResult> MovieIndex()
        {
            var now = DateTime.Now;
            var nowShowingMovies = await _context.TMovies
                .Where(m => m.FScheduleStart <= now && m.FScheduleEnd >= now).Take(4)
                .ToListAsync();

            var upcomingMovies = await _context.TMovies
                .Where(m => m.FScheduleStart > now)
                .ToListAsync();

            var movieViewModel = new CMovieFrontViewModel
            {
                NowShowingMovies = nowShowingMovies,
                UpcomingMovies = upcomingMovies
            };
            return View(movieViewModel);
        }

        // GET: movieFront/Details?id=
        public async Task<IActionResult> MovieDetails(int? id)
        {
            if (id == null || _context.TMovies == null)
            {
                return NotFound();
            }

            var tMovie = await _context.TMovies
                .Include(t => t.FRating)
                .Include(t => t.FSeries)
                .FirstOrDefaultAsync(m => m.FId == id);
            if (tMovie == null)
            {
                return NotFound();
            }

            return View(tMovie);
        }

        private bool TMovieExists(int id)
        {
          return (_context.TMovies?.Any(e => e.FId == id)).GetValueOrDefault();
        }
    }
}
