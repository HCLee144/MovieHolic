using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using prjMovieHolic.Models;

namespace prjMovieHolic.Controllers
{
    public class BackSuperController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_BackSys_LogIn))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(
                     new
                     {
                         controller = "Home",
                         action = "Index"
                     }));
            }
        }
    }
}
