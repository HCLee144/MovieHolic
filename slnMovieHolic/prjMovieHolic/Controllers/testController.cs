using Microsoft.AspNetCore.Mvc;

namespace prjMovieHolic.Controllers
{
    public class testController : Controller
    {
        public IActionResult Test()
        {
            //測試後台layout用
            return View();
        }
    }
}
