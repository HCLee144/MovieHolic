using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using prjMovieHolic.Models;
using System.Runtime.CompilerServices;

namespace prjMovieHolic.Controllers
{
    public class DashBoardController : BackSuperController
    {
        private readonly MovieContext _db;

        public DashBoardController(MovieContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var sum = _db.TOrders.Include(o => o.FSession).AsEnumerable().Where(o => o.FSession.FStartTime.Year == DateTime.Now.Year).Select(o => o.FTotalPrice).Sum();
            if (sum != null)
            {
                ViewBag.YearSum =  sum * 100 / 10000000;
                ViewBag.YearLabel1 = $"Until {DateTime.Now.ToString("MM/dd")}:";
                ViewBag.YearLabel2 = $"{sum:C3}";
            }
            var memberCount = _db.TMembers.Count();
            if (memberCount > 0)
                ViewBag.MemberCount = memberCount+ "人";
            return View();
        }

        public IActionResult getChartDataForSimpleIncomeByDay()
        {
            DateTime dateYesterDay = DateTime.Now.AddDays(-1).Date;
            DateTime startdate = DateTime.Now.AddDays(-7).Date;
            var q = _db.TOrders.Include(i => i.FSession).AsEnumerable()
                .Where(i => (i.FSession.FStartTime.Date <= dateYesterDay && i.FSession.FStartTime.Date >= startdate))
                .OrderByDescending(i=>i.FSession.FStartTime.Date)
                .GroupBy(i => i.FSession.FStartTime.Date.ToString("MM/dd")).Select(g => new {g.Key, group=g}).ToList();
            SimpleBarDataValues dataList = new SimpleBarDataValues();
            SimpleBarDataLabels labelList = new SimpleBarDataLabels();
            foreach (var item in q)
            {
                dataList.data.Add((int)item.group.Sum(i => i.FTotalPrice));
                labelList.categories.Add(item.Key);
            }
            return Json(new {data=dataList,labels=labelList });
        }

        public IActionResult getChartDataForSimpleIncomeByMonth()
        {
            DateTime dateYesterDay = DateTime.Now.AddDays(-1).Date;
            DateTime startdate = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)).AddMonths(-5); //先找這個月第一天，再找六個月前第一天
            var q = _db.TOrders.Include(i => i.FSession).AsEnumerable()
                .Where(i => (i.FSession.FStartTime.Date <= dateYesterDay && i.FSession.FStartTime.Date >= startdate))
                .OrderByDescending(i => i.FSession.FStartTime.Date)
                .GroupBy(i => i.FSession.FStartTime.ToString("yyyy/MM")).Select(g => new { g.Key, group = g }).ToList();
            SimpleBarDataValues dataList = new SimpleBarDataValues();
            SimpleBarDataLabels labelList = new SimpleBarDataLabels();
            foreach (var item in q)
            {
                dataList.data.Add((int)item.group.Sum(i => i.FTotalPrice));
                labelList.categories.Add(item.Key);
            }
            return Json(new { data = dataList, labels = labelList });
        }


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
