using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjMovieHolic.Models;
using prjMovieHolic.ViewModels;
using System.Diagnostics;
using System.Text.Json;

namespace prjMovieHolic.Controllers
{
    public class HomeController : Controller
    {
        private readonly MovieContext _context;

        public HomeController(MovieContext context)
        {
            _context = context;
        }

        // GET: movieFront
        public async Task<IActionResult> Index()
        {
            var now = DateTime.Now;
            var nowShowingMovies = await _context.TMovies
                .Where(m => m.FScheduleStart <= now && m.FScheduleEnd >= now)
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

            // 指定视图的路径
            return View("~/Views/movieFront/MovieDetails.cshtml", tMovie);
        }

        private bool TMovieExists(int id)
        {
            return (_context.TMovies?.Any(e => e.FId == id)).GetValueOrDefault();
        }


        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}