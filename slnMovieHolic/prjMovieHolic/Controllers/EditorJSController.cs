using Microsoft.AspNetCore.Mvc;

namespace prjMovieHolic.Controllers
{
    public class EditorJSController : SuperFrontController
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create() 
        {
            sessionCheck();
            return View();
        }
    }
}
