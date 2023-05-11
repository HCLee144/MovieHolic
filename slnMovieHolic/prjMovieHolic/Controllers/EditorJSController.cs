using Microsoft.AspNetCore.Mvc;

namespace prjMovieHolic.Controllers
{
    public class EditorJSController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create() 
        {
            return View();
        }
    }
}
