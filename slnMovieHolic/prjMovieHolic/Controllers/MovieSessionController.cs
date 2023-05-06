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
            //DateTime date = DateTime.Parse(vm.queryDate);
            //var datas =
            //    from s in _db.Sessions
            //    where s.StartTime.Date == date.Date
            //    select new
            //    {
            //        s.TheaterId,
            //        TheaterName = s.Theater.Theater1,
            //        s.MovieId,
            //        MovieName = s.Movie.FNameCht,
            //        s.StartTime,
            //        s.EndTime
            //    };
            var datas = new { Name = "子恒" };
            //string datasString = JsonSerializer.Serialize(datas, new JsonSerializerOptions
            //{
            //    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            //}) ;

            //return Content(datasString,"text/plain",Encoding.UTF8);
            return Json(datas);
        }
    }
}
