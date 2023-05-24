using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using prjMovieHolic.Models;
using prjMovieHolic.ViewModels;
using System.Data;

namespace prjMovieHolic.Controllers
{
    public class StatisticsController : BackSuperController
    {
        private readonly MovieContext _db;

        public StatisticsController(MovieContext db)
        {
            _db = db;
        }
        public IActionResult Member()
        {
            return View();
        }

        public IActionResult getChartDataForMemberAgePie()
        {
            PieData datas = new PieData();
            var q = _db.TMembers.AsEnumerable().OrderByDescending(member => member.FBirthDate).GroupBy(member => groupByAge((DateTime)member.FBirthDate, 0))
                .Select(g => new { g.Key, g }).ToList();
            foreach (var item in q)
            {
                datas.series.Add(item.g.Count());
                datas.labels.Add(item.Key);
            }
            return Json(datas);
        }


        public IActionResult getChartDataForMemberGenderPie()
        {
            PieData datas = new PieData();
            var q = _db.TMembers.Include(member => member.FGender).AsEnumerable().OrderBy(member => member.FGenderId).GroupBy(member => member.FGender.FGenderName)
                .Select(g => new { g.Key, g }).ToList();
            foreach (var item in q)
            {
                datas.series.Add(item.g.Count());
                datas.labels.Add(item.Key.ToString());
            }
            return Json(datas);
        }

        public IActionResult getChartDataForMemberAgeCoulmn()
        {
            List<BarSeries> datas = new List<BarSeries>();
            for (int i = 0; i < 2; i++)
            {
                BarSeries data = new BarSeries();
                int year = DateTime.Now.Year - i;
                data.name = year.ToString();
                data.data = getMemberAgeDataDistribution(year, i);
                datas.Add(data);
            }
            return Json(datas);
        }

        [NonAction]
        public List<BarSeriesData> getMemberAgeDataDistribution(int year, int diff)
        {
            var q = _db.TMembers.AsEnumerable()
                .Where(member => member.FCreatedDate.Year <= year)
                .OrderByDescending(member => member.FBirthDate)
                .GroupBy(member => groupByAge((DateTime)member.FBirthDate, diff)).ToList();
            List<BarSeriesData> datas = new List<BarSeriesData>();
            foreach (var group in q)
            {
                BarSeriesData data = new BarSeriesData();
                data.x = group.Key;
                data.y = group.Count();
                datas.Add(data);
            }
            return datas;
        }



        [NonAction]
        public string groupByAge(DateTime birth, int diff)
        {
            if (DateTime.Now.AddYears(-diff) <= birth.AddYears(18))
            {
                return "under 18";
            }
            else if (DateTime.Now.AddYears(-diff) <= birth.AddYears(30))
            {
                return "19~30";
            }
            else if (DateTime.Now.AddYears(-diff) <= birth.AddYears(40))
            {
                return "31~40";
            }
            else if (DateTime.Now.AddYears(-diff) <= birth.AddYears(50))
            {
                return "41~50";
            }
            else if (DateTime.Now.AddYears(-diff) <= birth.AddYears(60))
            {
                return "51~60";
            }
            else
            {
                return "over 60";
            }
        }


        public IActionResult Complaints()
        {
            mostComplaintsThisMonth();
            mostComplaintsLastMonth();
            return View();
        }

        [NonAction]
        public void mostComplaintsThisMonth()
        {
            DateTime start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).Date;
            var rawdatas = _db.TCnQlogs.Include(c => c.FCnQ).Include(c => c.FCnQ.FCnQtype).AsEnumerable()
                .Where(c => c.FStatusId == 1 && c.FTimeStamp.Date >= start && c.FCnQ.FIsComplaint == true)
                .GroupBy(c => c.FCnQ.FCnQtype.FCnQtype)
                .Select(g => new { g.Key, count = g.Count() });
            int Max = rawdatas.Max(data => data.count);
            var Types = rawdatas.Where(data => data.count == Max).Select(data => data.Key);
            ViewBag.mostThisMonth = String.Join("、", Types)+$" ({Max} 件)";
        }
        public void mostComplaintsLastMonth()
        {
            DateTime start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).Date.AddMonths(-1);
            DateTime end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).Date;
            var rawdatas = _db.TCnQlogs.Include(c => c.FCnQ).Include(c => c.FCnQ.FCnQtype).AsEnumerable()
                .Where(c => c.FStatusId == 1 && c.FTimeStamp.Date >= start &&c.FTimeStamp.Date<end && c.FCnQ.FIsComplaint == true)
                .GroupBy(c => c.FCnQ.FCnQtype.FCnQtype)
                .Select(g => new { g.Key, count = g.Count() });
            int Max = rawdatas.Max(data => data.count);
            var Types = rawdatas.Where(data => data.count == Max).Select(data => data.Key);
            ViewBag.mostLastMonth = String.Join("、", Types) + $" ({Max} 件)";
        }


        public IActionResult getChartDataForComplaints(int diff)
        {
            //取得所有客訴種類集合
            var complaintTypes = _db.TCnQtypes.OrderBy(type => type.FCnQtypeId).Select(type => type.FCnQtype).ToList();

            //前diff個月的起始時間
            DateTime start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-diff).Date;
            var rawdatas = _db.TCnQlogs.Include(c => c.FCnQ).Include(c => c.FCnQ.FCnQtype).AsEnumerable()
                .Where(c => (c.FStatusId == 1) && c.FTimeStamp.Date >= start&&c.FCnQ.FIsComplaint==true)
                .OrderBy(c => c.FCnQ.FCnQtypeId)
                .OrderByDescending(c => c.FTimeStamp)
                .GroupBy(c => c.FTimeStamp.ToString("yyyy/MM"))
                .Select(g => new { g.Key, group = g }).ToList();

            List<BarSeries> datas = new List<BarSeries>();
            foreach (var item in rawdatas)
            {
                BarSeries series = new BarSeries();
                series.name = item.Key;
                //每一個月份為一個series

                //每一個complaint為一個series的一筆data，先做出每種complaint
                foreach (var type in complaintTypes)
                {
                    BarSeriesData data = new BarSeriesData();
                    data.x = type;
                    data.y = 0;
                    series.data.Add(data);
                }

                foreach (var complaint in item.group)
                {

                    //每一筆rawdata在比對種類名稱後，填入數量
                    for (int i = 0; i < series.data.Count(); i++)
                    {
                        if (complaint.FCnQ.FCnQtype.FCnQtype == series.data[i].x)
                        {
                            series.data[i].y += 1;
                            break;
                        }
                    }
                }
                datas.Add(series);
            }
            return Json(datas);
        }
    }
}
