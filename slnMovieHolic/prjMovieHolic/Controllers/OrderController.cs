﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjMovieHolic.Models;
using prjMovieHolic.ViewModels;

namespace prjMovieHolic.Controllers
{
    public class OrderController : Controller
    {
        private MovieContext movieContext;
        public OrderController(MovieContext db)
        {
            movieContext = db;
        }
        public IActionResult ListSession(int? movieID)
        {
            if (movieID == null)
                return RedirectToAction("Index", "Home");
           var movie = movieContext.TMovies.Include(m => m.TSessions).Include(m=>m.TTypeLists).ThenInclude(t=>t.FType).Where(m => m.FId == movieID).FirstOrDefault();

            if (movie == null)
                return RedirectToAction("Index", "Home");

            CShoppingCartViewModel shoppingCart = new CShoppingCartViewModel();
            shoppingCart.tMovie = movie;

            shoppingCart.tTypeListNames = movie.TTypeLists.Where(t => t.FMovieId == movieID).Select(t=>t.FType.FNameCht).ToArray();

            shoppingCart.TypeListNames = getNames(shoppingCart.tTypeListNames);

        string[] days = new string[7] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };

            //將session時間取出星期幾(原先英文，轉數字)
            int selectDays = Convert.ToInt32(movie.TSessions.Select(s => s.FStartTime.DayOfWeek).FirstOrDefault());

            //將星期幾跟陣列對應，轉中文，連續取七天
            List<string> wholeWeekDays = new List<string>();

            while (wholeWeekDays.Count < 7)
            {
                wholeWeekDays.Add(days[selectDays++]);
                if (selectDays > 6)
                    selectDays = 0;
            }

            shoppingCart.weekDays = wholeWeekDays.ToArray();
            return View(shoppingCart);
        }



        public IActionResult queryByDate(string date)
        {
            string realDate = date.Substring(0, 5);
            var sessions = movieContext.TSessions.Include(s=>s.FTheater).AsEnumerable().Where(s => s.FStartTime.Date.ToString("MM/dd") == realDate);

            List<CShowSession> showSessions = new List<CShowSession>();
            foreach (var sessionItem in sessions)
            {
                bool verify = false;
                foreach (var item in showSessions)
                {
                    if (item.theaterName == sessionItem.FTheater.FTheater)
                    {
                        item.sessionIDandTime += $",{sessionItem.FSessionId}##{sessionItem.FStartTime.ToString("HH:mm")}";
                        verify = true;
                        break;
                    }
                }
                if(!verify)
                {
                    CShowSession showSession = new CShowSession();
                    showSession.theaterName = sessionItem.FTheater.FTheater;
                    showSession.sessionIDandTime = $"{sessionItem.FSessionId}##{sessionItem.FStartTime.ToString("HH:mm")}";

                    showSessions.Add(showSession);
                }
            }
            return Json(showSessions);
        }

        [NonAction]
       string getNames(Array data)
        {
            string result = "";
            foreach (var item in data)
            {
                result += " "+item;
            }
            return result;

        }
    }

    public class CShowSession
    {
        public string theaterName { get; set; }
        public string sessionIDandTime { get; set; }
    }
}
