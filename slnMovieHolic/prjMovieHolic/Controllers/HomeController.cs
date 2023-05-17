using Microsoft.AspNetCore.Mvc;
using prjMovieHolic.Models;
using System.Diagnostics;
using System.Text.Json;

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
            //登入用
            var userId = HttpContext.Session.GetInt32(CDictionary.SK_LOGIN_USER);
            var isUserLoggedIn = HttpContext.Session.GetInt32(CDictionary.SK_LOGIN_USER) != null;
            ViewBag.Login = isUserLoggedIn;
            ViewBag.UserId = userId;
            //todo 製作Session把Action和Controller存進去
            string controller = "Home";
            string view = "Index";
            //string json=JsonSerializer.Serialize(new { controller, view });
            HttpContext.Session.SetString(CDictionary.SK_CONTROLLER, controller);
            HttpContext.Session.SetString(CDictionary.SK_VIEW, view);
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