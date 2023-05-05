using Microsoft.AspNetCore.Mvc;
using prjMovieHolic.Models;

namespace prjMovieHolic.Controllers
{
    public class MovieSessionController : Controller
    {

        private MovieContext _db;
        public MovieSessionController() 
        {
            _db = new MovieContext();
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ViewSession()
        {

            return View();
        }
    }
}
