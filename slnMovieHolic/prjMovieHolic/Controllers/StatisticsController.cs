using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Execution;
using Microsoft.VisualBasic;
using prjMovieHolic.Models;

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
            ViewBag.GenderData = getChartDataForMemberGenderPie();
            ViewBag.AgeData= getChartDataForMemberAgePie();
            return View();
        }

        [NonAction]
        public string getChartDataForMemberAgePie()
        {
            string datas = "";
            var q = _db.TMembers.AsEnumerable().OrderBy(member => member.FBirthDate).GroupBy(member => groupByAge((DateTime)member.FBirthDate, 0))
                .Select(g=>new { g.Key,g }).ToList();
            foreach (var item in q)
            {
                datas += item.g.Count() + ",";
            }
            return datas;
        }


        [NonAction]
        public string getChartDataForMemberGenderPie()
        {
            string datas="";
            var q = _db.TMembers.AsEnumerable().OrderBy(member=>member.FGenderId).GroupBy(member => member.FGenderId)
                .Select(g =>g).ToList();
            foreach(var item in q)
            {
                datas += item.Count()+",";
            }
            return datas;
        }

        public IActionResult getChartDataForMemberAgeCoulmn()
        {
            List<CDataForMemberAge> datas = new List<CDataForMemberAge>(); 
            for (int i = 0; i < 2; i++)
            {
                CDataForMemberAge data = new CDataForMemberAge();
                int year = DateTime.Now.Year-i;
                data.name = year.ToString();
                data.data = getMemberAgeDataDistribution(year,i);
                datas.Add(data);
            }
            return Json(datas);
        }

        [NonAction]
        public List<MemberAgeData> getMemberAgeDataDistribution(int year, int diff)
        {
            var q = _db.TMembers.AsEnumerable()
                .Where(member => member.FCreatedDate.Year <= year)
                .OrderByDescending(member => member.FBirthDate)
                .GroupBy(member => groupByAge((DateTime)member.FBirthDate, diff)).ToList();
            List<MemberAgeData> datas = new List<MemberAgeData>();
            foreach (var group in q)
            {
                MemberAgeData data = new MemberAgeData();
                data.x = group.Key;
                data.y = group.Count();
                datas.Add(data);
            }
            return datas;
        }



        [NonAction]
        public string groupByAge(DateTime birth,int diff)
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

    }
}
