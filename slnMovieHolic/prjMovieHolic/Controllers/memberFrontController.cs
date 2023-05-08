using Microsoft.AspNetCore.Mvc;
using prjMovieHolic.Models;

namespace prjMovieHolic.Controllers
{
    public class memberFrontController : Controller
    {
        
        public IActionResult memberList(int? id)
        {
            MovieContext movieContext = new MovieContext();
            TMember memberData = movieContext.TMembers.FirstOrDefault(t => t.FMemberId == id);
            return View(memberData);
        }
        public IActionResult memberLogin() 
        {
            return View();
        }
    }
}
