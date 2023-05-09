using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using prjMovieHolic.Models;

namespace prjMovieHolic.Controllers
{
    public class SuperController : Controller
    {//todo 改成強行別?
        public void sessionCheck()
        {
            var userId = HttpContext.Session.GetInt32(CDictionary.SK_LOGIN_USER);
            var isUserLoggedIn = userId != null;
            ViewBag.Login = isUserLoggedIn;
            ViewBag.UserId = userId;
        }
    }
}
