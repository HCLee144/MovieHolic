using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using prjMovieHolic.Models;

namespace prjMovieHolic.Controllers
{
    public class SuperController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {//OnActionExecuting：當Action執行時，先來執行這段程式
            base.OnActionExecuting(context);
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_USER))
            {//若Session是空的，執行下列程式
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "memberFront",
                    action = "memberLogin",
                }));
            }
        }
    }
}
