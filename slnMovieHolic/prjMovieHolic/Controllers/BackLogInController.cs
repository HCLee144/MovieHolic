using Microsoft.AspNetCore.Mvc;

namespace prjMovieHolic.Controllers
{
    public class BackLogInController : Controller
    {
        public IActionResult LogIn()
        {
            return View();
        }
    }
}
