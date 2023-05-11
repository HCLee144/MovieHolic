using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjMovieHolic.Models;
using prjMovieHolic.ViewModels;

namespace prjMovieHolic.Controllers
{
    public class OrderFrontController : Controller
    {
        private MovieContext movieContext;
        public OrderFrontController(MovieContext db)
        {
            movieContext = db;
        }

        public IActionResult ListSession(int? movieID)
        {
            if (movieID == null)
                return RedirectToAction("Index", "Home");
           var movie = movieContext.TMovies.Include(m => m.TSessions).Include(m=>m.TTypeLists).ThenInclude(t=>t.FType).Include(m=>m.TDirectorLists).ThenInclude(t=>t.FDirector).Include(m=>m.TActorLists).ThenInclude(t=>t.FActor).Where(m => m.FId == movieID).FirstOrDefault();

            if (movie == null)
                return RedirectToAction("Index", "Home");

            CListSessionViewModel shoppingCart = new CListSessionViewModel();
            shoppingCart.tMovie = movie;
            shoppingCart.MovieName = movie.FNameCht;

            shoppingCart.tTypeListNames = movie.TTypeLists.Where(t => t.FMovieId == movieID).Select(t=>t.FType.FNameCht).ToArray();
            shoppingCart.TypeListNames = getNames(shoppingCart.tTypeListNames);

            shoppingCart.tDirectorListNames = movie.TDirectorLists.Where(t => t.FMovieId == movieID).Select(t => t.FDirector.FNameCht).ToArray();
            shoppingCart.DirectorListNames = getNames(shoppingCart.tDirectorListNames);

            shoppingCart.tActorListNames = movie.TActorLists.Where(t => t.FMovieId == movieID).Select(t => t.FActor.FNameCht).ToArray();
            shoppingCart.ActorListNames = getNames(shoppingCart.tActorListNames);

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

        public IActionResult SaveSelectedSessionID(int sessionID)
        {
            HttpContext.Session.SetInt32(CDictionary.SelectedSessionID, sessionID);
            return Content("Success");          
        }

        public IActionResult checkSessionSelected()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SelectedSessionID))
                return Content("有選到場次");
            else
                return Content("沒有選到場次");
        }

        public IActionResult ListTicketClass()
        {
            //顯示剛剛選的場次
            CListTicketViewModel shoppingCart = new CListTicketViewModel();
            int sessionID = (int)HttpContext.Session.GetInt32(CDictionary.SelectedSessionID);
            shoppingCart.selectedSessionID = sessionID;

            var sessionTime = movieContext.TSessions.FirstOrDefault(s => s.FSessionId == sessionID).FStartTime;
            string selectedSessionDateTime=sessionTime.ToString("MM/dd HH:mm");
            shoppingCart.selectedSessionDate = selectedSessionDateTime;

            var theater = movieContext.TSessions.Include(s => s.FTheater).FirstOrDefault(s => s.FSessionId == sessionID).FTheater;
            string selectedTheater = theater.FTheater;
            shoppingCart.selectedTheater=selectedTheater;

            var movie = movieContext.TSessions.Include(s => s.FMovie).FirstOrDefault(s => s.FSessionId == sessionID).FMovie;
            string movieName = movie.FNameCht;
            shoppingCart.selectedMovieName = movieName;


            //顯示選的票種價錢
            decimal moviePrice = (decimal)movie.FPrice;
            decimal 早場優惠 = 1;
            if (sessionTime.Hour <= 11)
                早場優惠=movieContext.TTicketClasses.FirstOrDefault(t => t.FTicketClassId == 5).FPriceRate;
            decimal 廳加價 = 1;
            if (theater.FTheaterId == 1)
                廳加價=movieContext.TTheaterClasses.FirstOrDefault(tc => tc.FTheaterClassId == 2).FPriceRate;
            decimal 一般價 = movieContext.TTicketClasses.FirstOrDefault(t => t.FTicketClassId == 1).FPriceRate;
            shoppingCart.oneNormalPrice = Convert.ToInt32(moviePrice*一般價 * 早場優惠 * 廳加價);
            decimal 學生價 = movieContext.TTicketClasses.FirstOrDefault(t => t.FTicketClassId == 2).FPriceRate;
            shoppingCart.oneStudentPrice = Convert.ToInt32(moviePrice * 學生價 * 早場優惠 * 廳加價);
            decimal 軍警價 = movieContext.TTicketClasses.FirstOrDefault(t => t.FTicketClassId == 5).FPriceRate;
            shoppingCart.oneSoldierPrice = Convert.ToInt32(moviePrice * 軍警價 * 早場優惠 * 廳加價);

            return View(shoppingCart);
        }

        public IActionResult ListSeat(CListSeatViewModel vm)
        {
            int n = vm.sessionID_seat;
            string x = vm.normalCount_seat.ToString();
            string y = vm.studentCount_seat.ToString();
            string z = vm.soldierCount_seat.ToString();
            return View();
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
