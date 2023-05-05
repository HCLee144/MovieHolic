using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjMovieHolic.Models;
using prjMovieHolic.ViewModels;
using System.Text;

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

        public IActionResult querySessionByDate(CMovieSessionViewModel vm)
        {
            DateTime date = DateTime.Parse(vm.queryDate);
            var datas =
                from s in _db.Sessions
                where s.StartTime.Date == date.Date
                select new
                {
                    s.TheaterId,
                    TheaterName = s.Theater.Theater1,
                    s.MovieId,
                    MovieName = s.Movie.FNameCht,
                    s.StartTime,
                    s.EndTime
                };
            return Json(datas);
        }
    }
}
