using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using prjMovieHolic.Models;
using prjMovieHolic.ViewModels;
using System.Collections.Generic;

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
            shoppingCart.MovieID = (int)movieID;
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
            //sessionCheck();
            shoppingCart.weekDays = wholeWeekDays.ToArray();
            return View(shoppingCart);
        }

        public IActionResult queryByDate(string date, int movieID)
        {
            //依據點選到的日期出現場次
            string realDate = date.Substring(0, 5);
            var sessions = movieContext.TSessions.Include(s=>s.FTheater).AsEnumerable().Where(s => s.FStartTime.Date.ToString("MM/dd") == realDate&&s.FMovieId==movieID);

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
            CListTicketViewModel vm = new CListTicketViewModel();
            int sessionID = getSessionID();
            vm.selectedSessionID = sessionID;

            vm.selectedSessionDate = getSessionDate(sessionID);
            vm.selectedSessionTime = getSessionTime(sessionID);
            var selectedSessionHour = getSessionHour(sessionID);

            var theater = getTheater(sessionID);
            vm.selectedTheater = theater.FTheater;

            var movie = getSessionMovie(sessionID);
            vm.selectedMovieName = movie.FNameCht;
            vm.selectedMovieEngName = movie.FNameEng;

            //顯示選的票種價錢
            decimal moviePrice = (decimal)movie.FPrice;
            decimal 早場優惠 = 1;
            if (int.Parse(selectedSessionHour) <= 11)
                早場優惠=movieContext.TTicketClasses.FirstOrDefault(t => t.FTicketClassId == 5).FPriceRate;
            decimal 廳加價 = 1;
            if (theater.FTheaterId == 1)
                廳加價=movieContext.TTheaterClasses.FirstOrDefault(tc => tc.FTheaterClassId == 2).FPriceRate;
            decimal 一般價 = movieContext.TTicketClasses.FirstOrDefault(t => t.FTicketClassId == 1).FPriceRate;
            vm.oneNormalPrice = Convert.ToInt32(moviePrice*一般價 * 早場優惠 * 廳加價);
            decimal 學生價 = movieContext.TTicketClasses.FirstOrDefault(t => t.FTicketClassId == 2).FPriceRate;
            vm.oneStudentPrice = Convert.ToInt32(moviePrice * 學生價 * 早場優惠 * 廳加價);
            decimal 軍警價 = movieContext.TTicketClasses.FirstOrDefault(t => t.FTicketClassId == 5).FPriceRate;
            vm.oneSoldierPrice = Convert.ToInt32(moviePrice * 軍警價 * 早場優惠 * 廳加價);

            return View(vm);
        }

        public IActionResult ListSeat(CListSeatViewModel vm)
        {
            int n = vm.sessionID_seat;
            int x = vm.normalCount_seat;
            int y = vm.studentCount_seat;
            int z = vm.soldierCount_seat;

            //將選的票種數量存進session
            string tickets = $"{x},{y},{z}";
            HttpContext.Session.SetString(CDictionary.SelectedTicketClass, tickets);

            //選到的電影
            vm.movieName = movieContext.TSessions.Include(s=>s.FMovie).FirstOrDefault(s => s.FSessionId == n).FMovie.FNameCht;
            //選到的日期
            vm.sessionDate = movieContext.TSessions.FirstOrDefault(s => s.FSessionId == n).FStartTime.ToString("MM/dd");
            //選到的時間
            vm.sessionTime = movieContext.TSessions.FirstOrDefault(s => s.FSessionId == n).FStartTime.ToString("HH:mm");
            //全部票數
            vm.totalTickets = (x + y + z).ToString();
            string s = vm.totalTickets;

            //選到的廳
            var theaterID=movieContext.TSessions.FirstOrDefault(s => s.FSessionId == n).FTheaterId;
            vm.theaterID = theaterID.ToString();
            vm.theaterName = movieContext.TTheaters.FirstOrDefault(t => t.FTheaterId == theaterID).FTheater;


            //所有這個場次的seat
            var seats=movieContext.TSessions.Include(s=>s.FTheater).ThenInclude(t=>t.TSeats).FirstOrDefault(s => s.FSessionId == n).FTheater.TSeats;

            //非座位區的所有ID
            var nonSeats=seats.Where(s => s.FSeatStatusId == 2).Select(s=>s.FSeatId);

            //愛心座位的所有ID
            var diabledSeats = seats.Where(s => s.FSeatStatusId == 3).Select(s=>s.FSeatId);

            //已被選走的座位
            var selectedSeats=movieContext.TOrderDetails.Include(od=>od.FOrder).Where(od => od.FOrder.FSessionId == n).Select(od => od.FSeatId);

            int[] seat = new int[400];
            //假設數字1預設為未售，2為非座位，3為愛心座位，4為被選走座位
            foreach (var item in nonSeats)
            {
                int changedSeatID = item - 400 * (theaterID - 1);
                seat[changedSeatID - 1] = 2;
            }
            foreach (var item in diabledSeats)
            {
                int changedSeatID = item - 400 * (theaterID - 1);
                seat[changedSeatID - 1] = 3;
            }
            foreach (var item in selectedSeats)
            {
                {
                    int changedSeatID = item - 400 * (theaterID - 1);
                    seat[changedSeatID - 1] = 4;
                }
            }
            for(int i=0;i<seat.Length;i++)
            {
                if (seat[i] == 0)
                    seat[i] = 1;
            }
            vm.seatStatus = seat;

            return View(vm);
        }

        public IActionResult ListProduct()
        {
            var product = movieContext.TProducts.ToList();

            //將產品分門別類
            CListProductViewModel vm = new CListProductViewModel();
            List<CTProductWrap> drinks = new List<CTProductWrap>();
            List<CTProductWrap> popcorns = new List<CTProductWrap>();
            List<CTProductWrap> desserts = new List<CTProductWrap>();
            foreach (var item in product)
            {
                if(item.FCategoryId==1)
                {
                    CTProductWrap productWrap = new CTProductWrap();
                    productWrap.product = movieContext.TProducts.FirstOrDefault(p => p.FProductId == item.FProductId);
                    productWrap.FProductId = movieContext.TProducts.FirstOrDefault(p => p.FProductId == item.FProductId).FProductId;
                    productWrap.FProductName = movieContext.TProducts.FirstOrDefault(p => p.FProductId == item.FProductId).FProductName;
                    productWrap.FProductPrice = movieContext.TProducts.FirstOrDefault(p => p.FProductId == item.FProductId).FProductPrice;
                    productWrap.FImage = movieContext.TProducts.FirstOrDefault(p => p.FProductId == item.FProductId).FImage;
                    drinks.Add(productWrap);
                }
                if(item.FCategoryId==2)
                {
                    CTProductWrap productWrap = new CTProductWrap();
                    productWrap.product = movieContext.TProducts.FirstOrDefault(p => p.FProductId == item.FProductId);
                    productWrap.FProductId = movieContext.TProducts.FirstOrDefault(p => p.FProductId == item.FProductId).FProductId;
                    productWrap.FProductName = movieContext.TProducts.FirstOrDefault(p => p.FProductId == item.FProductId).FProductName;
                    productWrap.FProductPrice = movieContext.TProducts.FirstOrDefault(p => p.FProductId == item.FProductId).FProductPrice;
                    productWrap.FImage = movieContext.TProducts.FirstOrDefault(p => p.FProductId == item.FProductId).FImage;
                    popcorns.Add(productWrap);
                }
                if (item.FCategoryId == 3)
                {
                    CTProductWrap productWrap = new CTProductWrap();
                    productWrap.product = movieContext.TProducts.FirstOrDefault(p => p.FProductId == item.FProductId);
                    productWrap.FProductId = movieContext.TProducts.FirstOrDefault(p => p.FProductId == item.FProductId).FProductId;
                    productWrap.FProductName = movieContext.TProducts.FirstOrDefault(p => p.FProductId == item.FProductId).FProductName;
                    productWrap.FProductPrice = movieContext.TProducts.FirstOrDefault(p => p.FProductId == item.FProductId).FProductPrice;
                    productWrap.FImage = movieContext.TProducts.FirstOrDefault(p => p.FProductId == item.FProductId).FImage;
                    desserts.Add(productWrap);
                }
            }

            vm.drinkCategory = drinks;
            vm.popcornCategory = popcorns;
            vm.dessertCategory = desserts;

            //抓電影名字
            int sessionID = getSessionID();
            vm.selectedSessionID = sessionID;
            var movie=getSessionMovie(sessionID);
            vm.selectedMovieName = movie.FNameCht;

            //抓時間
            vm.selectedSessionDate=getSessionDate(sessionID);
            vm.selectedSessionTime = getSessionTime(sessionID);

            //抓影廳
            var theater = getTheater(sessionID);
            vm.selectedTheaterID = theater.FTheaterId;
            vm.selectedTheaterName = theater.FTheater;

            //抓人數
            string json_tickets=HttpContext.Session.GetString(CDictionary.SelectedTicketClass);
            string[] tickets=json_tickets.Split(",");

            int totalCount = 0; 
            foreach (var item in tickets)
                totalCount += Convert.ToInt32(item);
            vm.ticketCounts = totalCount.ToString();


            //抓座位
            List<string> showSeats=getSeatNames(theater.FTheaterId);
            vm.selectedSeats = showSeats;

            return View(vm);

        }

        public IActionResult saveSeatIDinSession(int correctSeatID, int totalTickets, string selected)
        {
            //先call出之前的session
            List<int> selectedSeats;
            string json;
            if (HttpContext.Session.Keys.Contains(CDictionary.SelectedSeatID))
            {
                json = HttpContext.Session.GetString(CDictionary.SelectedSeatID);
                selectedSeats = System.Text.Json.JsonSerializer.Deserialize<List<int>>(json);
            }
            else
                selectedSeats = new List<int>(); 

            //判斷是否有被選擇過
            if(selected=="1")
            {
                selectedSeats.Remove(correctSeatID);
                json = System.Text.Json.JsonSerializer.Serialize(selectedSeats);
                HttpContext.Session.SetString(CDictionary.SelectedSeatID, json);
                int seatCount = totalTickets - selectedSeats.Count();
                return Content("尚可選座位：" + seatCount);
            }
            else
            {
                if (selectedSeats.Count() < totalTickets)
                {
                    selectedSeats.Add(correctSeatID);
                    json = System.Text.Json.JsonSerializer.Serialize(selectedSeats);
                    HttpContext.Session.SetString(CDictionary.SelectedSeatID, json);
                    int seatCount = totalTickets - selectedSeats.Count();
                    return Content("尚可選座位：" + seatCount);
                }
                else
                    return Content("座位人數已滿");
                
            }
        }

        public IActionResult saveProductCount(string products)
        {
            string json=System.Text.Json.JsonSerializer.Serialize(products);
            HttpContext.Session.SetString(CDictionary.SelectedProductCount, json);
            return Content("購買食物成功");
        }

        public IActionResult ListOrderDetails()
        {
            CListOrderDetailsViewModel vm = new CListOrderDetailsViewModel();

            //選擇到的票種張數
            string json_tickets=HttpContext.Session.GetString(CDictionary.SelectedTicketClass);
            string[] tickets = json_tickets.Split(",");
            string[] ticketsNames = new string[] { "一般票", "學生票", "軍警票" };
            string selectedTickets="";
            for(int i=0;i<tickets.Length;i++)
            {
                if (Convert.ToInt32(tickets[i]) > 0)
                    selectedTickets = $"{ticketsNames[i]}:{tickets[i]}張,";
            }
            if (selectedTickets.Trim().Substring(selectedTickets.Trim().Length - 1, 1) == ",")
                selectedTickets = selectedTickets.Trim().Substring(0, selectedTickets.Trim().Length - 1);
            vm.tickets = selectedTickets;

            //選擇到的電影名稱、日期時間
            int sessionID= getSessionID();
            var movie = getSessionMovie(sessionID);
            vm.selectedMovieName = movie.FNameCht;
            vm.selectedMovieID = movie.FId;
            vm.selecteDate = getSessionDate(sessionID) + getSessionTime(sessionID);

            //選擇到的廳院
            var theater = getTheater(sessionID);
            string selectedTheater = theater.FTheater;
            vm.theaterName = selectedTheater;
            int theaterID = theater.FTheaterId;

            //選擇到的座位(correctSeatID)
            List<string> showSeats = getSeatNames(theaterID);
            vm.seats = showSeats;

            //選擇到的餐點
            string json_set=HttpContext.Session.GetString(CDictionary.SelectedProductCount);
            string product=System.Text.Json.JsonSerializer.Deserialize<string>(json_set);
            string[] productCounts=product.Split(",");
            var productNames = movieContext.TProducts.Select(p => p.FProductName).ToArray();
            List<string> showProducts = new List<string>();
            for(int i=0;i<productCounts.Length;i++)
            {
                if (Convert.ToInt32(productCounts[i].Trim())>0)
                {
                    string thisProduct = $"{productNames[i]}X{productCounts[i]}";
                    showProducts.Add(thisProduct);
                }
            }
            vm.set = showProducts;

            return View(vm);
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
        
        int getSessionID()
        {
            int sessionID = (int)HttpContext.Session.GetInt32(CDictionary.SelectedSessionID);
            return sessionID;
        }

        TMovie getSessionMovie(int sessionID)
        {
            var movie=movieContext.TSessions.Include(s => s.FMovie).FirstOrDefault(s => s.FSessionId == sessionID).FMovie;
            return movie;
        }
        string getSessionDate(int sessionID)
        {
            string selectedMonth = movieContext.TSessions.FirstOrDefault(s => s.FSessionId == sessionID).FStartTime.ToString("MM");
            string selectedDate = movieContext.TSessions.FirstOrDefault(s => s.FSessionId == sessionID).FStartTime.ToString("dd");
            string selecteDate = $"{selectedMonth}月{selectedDate}日";
            return selecteDate;
        }

        string getSessionTime(int sessionID)
        {
            string selectedHour = movieContext.TSessions.FirstOrDefault(s => s.FSessionId == sessionID).FStartTime.ToString("HH");
            string selectedMinute = movieContext.TSessions.FirstOrDefault(s => s.FSessionId == sessionID).FStartTime.ToString("mm");
            string selecteTime = $"{selectedHour}時{selectedMinute}分";
            return selecteTime;
        }

        string getSessionHour(int sessionID)
        {
            string selectedHour = movieContext.TSessions.FirstOrDefault(s => s.FSessionId == sessionID).FStartTime.ToString("HH");
            return selectedHour;
        }
        TTheater getTheater(int sessionID)
        {
            var theater = movieContext.TSessions.Include(s => s.FTheater).FirstOrDefault(s => s.FSessionId == sessionID).FTheater;
            return theater;
        }

        List<string> getSeatNames(int theaterID)
        {
            string json_seats = HttpContext.Session.GetString(CDictionary.SelectedSeatID);
            List<int> seats = System.Text.Json.JsonSerializer.Deserialize<List<int>>(json_seats);
            List<string> list = new List<string>();
            foreach(var item in seats)
            {
                int row;
                int col;
                int seatIDinTheater = item-(theaterID - 1) * 400;
                if((seatIDinTheater)%20!=0)
                {
                    row = (seatIDinTheater) / 20+1;
                    col = (seatIDinTheater) % 20;
                }
                else
                {
                    row = (seatIDinTheater) / 20;
                    col = 20;
                }
                list.Add($"{row}排{col}");
            }
            return list;
        }
    }

    public class CShowSession
    {
        public string theaterName { get; set; }
        public string sessionIDandTime { get; set; }
    }


}
