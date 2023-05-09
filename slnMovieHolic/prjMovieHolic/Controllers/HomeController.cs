using Microsoft.AspNetCore.Mvc;
using prjMovieHolic.Models;
using System.Diagnostics;

namespace prjMovieHolic.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32(CDictionary.SK_LOGIN_USER);
            var isUserLoggedIn = HttpContext.Session.GetInt32(CDictionary.SK_LOGIN_USER)!= null;
            ViewBag.Login = isUserLoggedIn;
            ViewBag.UserId = userId;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}