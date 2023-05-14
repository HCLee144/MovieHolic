using Microsoft.AspNetCore.Mvc;

namespace prjMovieHolic.Controllers
{
    public class DashBoardController : Controller
    {
        public IActionResult DashBoard()
        {
            return View();
        }

        public IActionResult getChartDataForMovieIncome()
        {

            return View();
        }
    }
}
