using Microsoft.AspNetCore.Mvc;
using prjMovieHolic.Models;
using prjMovieHolic.ViewModels;

namespace prjMovieHolic.Controllers
{
    public class BackLogInController : Controller
    {

        private readonly MovieContext _db;

        public BackLogInController(MovieContext db)
        {
            _db = db;
        }
        public IActionResult LogIn()
        {
            if(HttpContext.Session.Keys.Contains(CDictionary.SK_BackSys_LogIn))
                return RedirectToAction("Index", "DashBoard");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LogIn(string txtAccount, string txtPassword)
        {
            TEmployee employee = _db.TEmployees.FirstOrDefault(t => t.FEmployeeAccount.Equals(txtAccount));
            if (employee != null && employee.FPassword.Equals(txtPassword))
            {
                HttpContext.Session.SetInt32(CDictionary.SK_BackSys_LogIn, employee.FEmployeeId);
                return RedirectToAction("Index", "DashBoard");
            }
            return View();
        }
        public IActionResult IsLogin(string txtAccount, string txtPassword)
        {
            bool isSent = _db.TEmployees.Where(t => t.FEmployeeAccount == txtAccount && t.FPassword == txtPassword).ToList().Any();
            if (!isSent)
            {
                return Json(new { success = false, message = "輸入的帳號或密碼錯誤，請重新輸入。" });
            }
            else
            {
                return Json(new { success = true, message = "" });
            }
        }

        public IActionResult LogOut() 
        {
            HttpContext.Session.Remove(CDictionary.SK_BackSys_LogIn);
            return RedirectToAction("Index","Home");
        }
    }
}
