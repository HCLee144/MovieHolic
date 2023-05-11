﻿using Microsoft.AspNetCore.Mvc;
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

        public IActionResult getChartDataForSessionByDate(CSessionBackViewModel vm)
        {
            try
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
            catch (Exception ex)
            {
                return Json(null);
            }
        }

        public IActionResult getSessionByTheaterAndStart(CSessionBackViewModel vm)
        {
            try
            {
                int TheaterID = _db.TTheaters.Where(t => t.FTheater == vm.selectedSessionTheaterName).Select(t => t.FTheaterId).FirstOrDefault();
                DateTime startTime = DateTime.Parse(vm.selectedSessionStartString);
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
                datas.MovieLength = session.FMovie.FShowLength+" 分鐘";
                datas.MoviePosterPath =session.FMovie.FPosterPath;
                return Json(datas);
            }
            catch (Exception ex)
            {
                return Json(null);
            }
        }

        public IActionResult loadSelectTheater()
        {
            var q = _db.TTheaters.Select(t => new
            {
                t.FTheaterId,
                t.FTheater
            }) ;
            return Json(q);
        }

        public IActionResult loadSelectMovie(string? createDate)
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
    }
}
