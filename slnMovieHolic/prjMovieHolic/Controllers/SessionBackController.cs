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
    public class SessionBackController : Controller
    {
        private readonly MovieContext _db;

        public SessionBackController(MovieContext db)
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

        public IActionResult getChartDataForSessionByDate(CMovieSessionViewModel vm)
        {
            DateTime date = DateTime.Parse(vm.queryDate);
            var rawDatas =
                from s in _db.TSessions.AsEnumerable()
                where s.FStartTime.Date == date.Date
                group s by s.FMovieId into g
                select new
                {
                    movieID = g.Key,
                    g
                };
            List<CDataForSessionChart> datas = new List<CDataForSessionChart>();
            foreach (var rawData in rawDatas)
            {
                string movieName = _db.TMovies.Where(m => m.FId == rawData.movieID).First().FNameCht;
                if (!datas.Where(data => data.name == movieName).Any())
                {
                    datas.Add(new CDataForSessionChart { name = movieName });
                    //如果datas裡還沒有這個電影名的資料，新增物件
                }
                foreach (var session in rawData.g)
                {
                    SessionTheaterAndTimeData sessionData = new SessionTheaterAndTimeData();
                    sessionData.x = _db.TTheaters.Where(m => m.FTheaterId == session.FTheaterId).First().FTheater;
                    sessionData.y = new long[]
                    {
                        new DateTimeOffset(session.FStartTime.AddHours(8)).ToUnixTimeMilliseconds(),
                         new DateTimeOffset(session.FEndTime.AddHours(8)).ToUnixTimeMilliseconds()
                    };
                    datas.Where(data => data.name == movieName).First().data.Add(sessionData);
                }
            }
            return Json(datas);
        }

        //public IActionResult get
    }
}
