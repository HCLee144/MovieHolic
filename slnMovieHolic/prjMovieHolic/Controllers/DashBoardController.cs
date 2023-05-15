using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public IActionResult getChartDataForMovieIncome()
        {
            DateTime dateYesterDay = DateTime.Now.AddDays(-1).Date;
            DateTime dateWeekAgo = DateTime.Now.AddDays(-7).Date;
            var q = _db.TOrders.Include(i => i.FSession).Include(i => i.FSession.FMovie).AsEnumerable()
                .Where(i => (i.FSession.FStartTime <= dateYesterDay && i.FSession.FStartTime >= dateWeekAgo))
                .GroupBy(i => i.FSession.FMovie.FNameCht).Select(g=> new {g.Key,group = g});
            //CDataForMovieIncome datas = new
            return View();
        }
    }
}
