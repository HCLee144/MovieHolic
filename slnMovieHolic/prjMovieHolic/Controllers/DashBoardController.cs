using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using prjMovieHolic.Models;

namespace prjMovieHolic.Controllers
{
    public class DashBoardController : Controller
    {
        private readonly MovieContext _db;

        public DashBoardController(MovieContext db)
        {
            _db = db;
        }
        public IActionResult DashBoard()
        {
            return View();
        }

        //public IActionResult getChartDataForMovieIncome()
        //{
        //    DateTime dateYesterDay = DateTime.Now.AddDays(-1).Date; //昨天的場次，從今日的0時之前
        //    DateTime dateWeekAgo = DateTime.Now.AddDays(-7).Date;
        //    var q = _db.TOrders.Include(i => i.FSession).Include(i => i.FSession.FMovie).AsEnumerable()
        //        .Where(i => (i.FSession.FStartTime.Date <= dateYesterDay && i.FSession.FStartTime.Date >= dateWeekAgo))
        //        .OrderByDescending(i=>i.FSession.FMovie.FScheduleStart)
        //        .GroupBy(i => i.FSession.FMovie.FNameCht).Select(g=> new {g.Key,group = g}).ToList();
        //    List<CDataForMovieIncome> chartData = new List<CDataForMovieIncome>();
        //    foreach(var group in q)
        //    {
        //        CDataForMovieIncome movieData = new CDataForMovieIncome();
        //        //按照電影分series
        //        movieData.name = group.Key;
        //        //每部電影按照前一天到前七天計算票房總合
        //        movieData.data = new List<IncomeDataPerDay>();
        //        foreach(var order in group.group)
        //        {
        //            IncomeDataPerDay income=null;
        //            if (!movieData.data.Where(data => data.x == order.FSession.FStartTime.ToString("MM-dd")).Any())
        //            {
        //                income = new IncomeDataPerDay();
        //                income.x = order.FSession.FStartTime.ToString("MM-dd");
        //                income.y = (int)order.FTotalPrice;
        //                movieData.data.Add(income);
        //            }
        //            else
        //            {
        //                movieData.data.Where(data => data.x == order.FSession.FStartTime.ToString("MM-dd")).First().y += (int)order.FTotalPrice;
        //            }
        //        }
        //        chartData.Add(movieData);
        //    }
        //    return Json(chartData);
        //}
        public IActionResult getChartDataForMovieIncome()
        {
            DateTime dateYesterDay = DateTime.Now.AddDays(-1).Date; 
            DateTime dateWeekAgo = DateTime.Now.AddDays(-7).Date;
            var q = _db.TOrders.Include(i => i.FSession).Include(i => i.FSession.FMovie).AsEnumerable()
                .Where(i => (i.FSession.FStartTime.Date <= dateYesterDay && i.FSession.FStartTime.Date >= dateWeekAgo))
                .OrderByDescending(i => i.FSession.FMovie.FScheduleStart)
                .GroupBy(i => i.FSession.FMovie.FNameCht).Select(g => new { g.Key, group = g }).ToList();
            List<CDataForMovieIncome> chartData = new List<CDataForMovieIncome>();
            foreach (var group in q)
            {
                CDataForMovieIncome movieData = new CDataForMovieIncome();
                //按照電影分series
                movieData.name = group.Key;
                //每部電影按照前一天到前七天計算票房總合
                movieData.data = new List<IncomeDataPerDay>();
                for(int i = 0; i < 7; i++)
                {
                    IncomeDataPerDay income = new IncomeDataPerDay()
                    {
                        x = dateYesterDay.AddDays(-i).ToString("MM/dd"),
                        y = 0
                    };
                    movieData.data.Add(income);
                }
                foreach (var order in group.group)
                {
                    int span = (dateYesterDay-order.FSession.FStartTime.Date).Days;
                    movieData.data[span].y += (int)order.FTotalPrice;
                    
                }
                chartData.Add(movieData);
            }
            return Json(chartData);
        }
    }
}
