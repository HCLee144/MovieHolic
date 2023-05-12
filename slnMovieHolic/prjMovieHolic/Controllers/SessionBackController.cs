using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
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

        public IActionResult ViewSession(int? message)
        {
            if (message != null)
                ViewBag.Error = message;
            return View();
        }

        public IActionResult getChartDataForSessionByDate(string? queryDate)
        {
            try
            {
                DateTime date = DateTime.Parse(queryDate);
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
            catch (Exception ex)
            {
                return RedirectToAction("ViewSession");
            }
        }

        public IActionResult getSessionByTheaterAndStart(string? selectedSessionTheaterName, string? selectedSessionStartString)
        {
            try
            {
                int TheaterID = _db.TTheaters.Where(t => t.FTheater == selectedSessionTheaterName).Select(t => t.FTheaterId).FirstOrDefault();
                DateTime startTime = DateTime.Parse(selectedSessionStartString);
                var session = _db.TSessions.Include(s => s.FMovie).Where(s => s.FTheaterId == TheaterID && s.FStartTime == startTime).FirstOrDefault();
                if (session == null)
                {
                    return Json(null);
                }
                CSessionBackViewModel datas = new CSessionBackViewModel();
                datas.SessionID = session.FSessionId;
                datas.StartTime = session.FStartTime;
                datas.EndTime = session.FEndTime;
                datas.hasOrder = _db.TOrders.Where(o => o.FSessionId == session.FSessionId).Any();
                datas.MovieName = session.FMovie.FNameCht;
                datas.MovieLength = session.FMovie.FShowLength + " 分鐘";
                datas.MoviePosterPath = session.FMovie.FPosterPath;
                return Json(datas);
            }
            catch (Exception ex)
            {
                return RedirectToAction("ViewSession");
            }
        }

        public IActionResult loadSelectTheater()
        {
            try
            {
                var q = _db.TTheaters.Select(t => new
                {
                    t.FTheaterId,
                    t.FTheater
                });
                return Json(q);
            }
            catch (Exception ex)
            {
                return RedirectToAction("ViewSession");
            }
        }

        public IActionResult loadSelectMovie(string? createDate)
        {
            try
            {
                DateTime date = DateTime.Parse(createDate);
                var q = _db.TMovies.Where(m => m.FScheduleStart <= date && m.FScheduleEnd > date).Select(m => new
                {
                    m.FId,
                    m.FNameCht,
                    m.FShowLength
                });
                return Json(q);
            }
            catch (Exception ex)
            {
                return RedirectToAction("ViewSession");
            }
        }

        public IActionResult create(CSessionBackViewModel vm)
        {
            try
            {
                DateTime startTime = DateTime.Parse(vm.createDate) + vm.StartTime.TimeOfDay;
                int length = (int)_db.TMovies.Where(m => m.FId == vm.MovieID).Select(m => m.FShowLength).First();
                TimeSpan lengthMinute = TimeSpan.FromMinutes(length);
                DateTime endTime = startTime + lengthMinute;

                var q = _db.TSessions.Where(s => s.FStartTime.Date == startTime.Date && s.FTheaterId == vm.TheaterID)
                    .Select(s => new { s.FStartTime, s.FEndTime });

                bool isOverLapped = false;
                TimeSpan span = TimeSpan.FromMinutes(5);
                foreach (var s in q)
                {
                    if (startTime > (s.FStartTime - span) && startTime < (s.FEndTime + span))
                    {
                        isOverLapped = true;
                        break;
                    }
                    if (endTime > (s.FStartTime - span) && endTime < (s.FEndTime + span))
                    {
                        isOverLapped = true;
                        break;
                    }
                }
                if (isOverLapped)
                    return RedirectToAction("ViewSession", "SessionBack", new { message = 1 });

                TSession session = new TSession();
                session.FTheaterId = (int)vm.TheaterID;
                session.FMovieId = (int)vm.MovieID;
                session.FStartTime = startTime;
                session.FEndTime = endTime;
                _db.TSessions.Add(session);
                _db.SaveChanges();
                return RedirectToAction("ViewSession");
            }
            catch (Exception ex)
            {
                return RedirectToAction("ViewSession");
            }
        }

        public IActionResult delete(int? SessionID)
        {
            if (SessionID == null)
                return RedirectToAction("ViewSession");
            _db.TSessions.Where(s => s.FSessionId == SessionID).ExecuteDelete();
            _db.SaveChanges();
            return RedirectToAction("ViewSession");
        }

    }
}
