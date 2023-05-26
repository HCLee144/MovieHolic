using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using prjMovieHolic.Models;

namespace prjMovieHolic.Controllers
{
    public class SuperFrontController : Controller
    {
        public void sessionCheck()
        {
            var userId = HttpContext.Session.GetInt32(CDictionary.SK_LOGIN_USER);
            var userName=HttpContext.Session.GetString(CDictionary.SK_LOGIN_USER_NAME);
            var isUserLoggedIn = userId != null;
            ViewBag.Login = isUserLoggedIn;
            ViewBag.UserId = userId;
            ViewBag.userName = userName;

        }
    }
}
