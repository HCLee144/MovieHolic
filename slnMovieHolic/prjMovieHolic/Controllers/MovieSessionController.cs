using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using Newtonsoft.Json;
using prjMovieHolic.Models;
using prjMovieHolic.ViewModels;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace prjMovieHolic.Controllers
{
    public class MovieSessionController : Controller
    {
        private readonly MovieContext _db;

        public MovieSessionController(MovieContext db)
        {
            _db = db;
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
                from s in _db.TSessions
                where s.FStartTime.Date == date.Date
                select new
                {
                    s.FTheaterId,
                    s.FMovieId,
                    MovieName = s.FMovie.FNameCht,
                    s.FStartTime,
                    s.FEndTime
                };

            return Json(datas);
        }
    }
}
