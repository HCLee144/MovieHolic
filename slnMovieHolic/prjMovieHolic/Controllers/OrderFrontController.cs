using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using prjMovieHolic.Models;
using prjMovieHolic.ViewModels;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Drawing;
using System.Net.NetworkInformation;



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
            try
            {
                ClearAllSession();
                //先判斷是否有場次
                var session = movieContext.TSessions.Where(s => s.FMovieId == movieID).
                    Where(s => s.FStartTime.Date > DateTime.Now.Date).ToList();
                if (session.Count == 0)
                    return RedirectToAction("Index", "Home");


                //判斷會員登入或會員中心
                bool verify_checkLogIn = sessionCheck();

                if (movieID == null)
                    return RedirectToAction("Index", "Home");
                var movie = movieContext.TMovies.Include(m => m.TSessions).Include(m => m.TTypeLists).
                    ThenInclude(t => t.FType).Include(m => m.TDirectorLists).ThenInclude(t => t.FDirector).
                    Include(m => m.TActorLists).ThenInclude(t => t.FActor).Where(m => m.FId == movieID).FirstOrDefault();

                if (movie == null)
                    return RedirectToAction("Index", "Home");


                CListSessionViewModel vm = new CListSessionViewModel();
                vm.tMovie = movie;
                vm.MovieID = (int)movieID;
                vm.MovieName = movie.FNameCht;

                vm.tTypeListNames = movie.TTypeLists.Where(t => t.FMovieId == movieID).Select(t => t.FType.FNameCht).ToArray();
                vm.TypeListNames = getNames(vm.tTypeListNames);

                vm.tDirectorListNames = movie.TDirectorLists.Where(t => t.FMovieId == movieID).Select(t => t.FDirector.FNameCht).ToArray();
                vm.DirectorListNames = getNames(vm.tDirectorListNames);

                vm.tActorListNames = movie.TActorLists.Where(t => t.FMovieId == movieID).Select(t => t.FActor.FNameCht).ToArray();
                vm.ActorListNames = getNames(vm.tActorListNames);

                string[] days = new string[7] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };

                //將session時間取出星期幾(原先英文，轉數字)
                int selectDays = Convert.ToInt32(movie.TSessions.Where(s => s.FStartTime.Date > DateTime.Now.Date).Select(s => s.FStartTime.DayOfWeek).FirstOrDefault());

                //將星期幾跟陣列對應，轉中文，連續取七天
                List<string> wholeWeekDays = new List<string>();

                while (wholeWeekDays.Count < 7)
                {
                    wholeWeekDays.Add(days[selectDays++]);
                    if (selectDays > 6)
                        selectDays = 0;
                }

                vm.weekDays = wholeWeekDays.ToArray();
                return View(vm);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }

        }

        public IActionResult queryByDate(string date, int movieID)
        {
            try
            {
                //依據點選到的日期出現場次
                string realDate = date.Substring(0, 5);
                var sessions = movieContext.TSessions.Include(s => s.FTheater).AsEnumerable().
                    OrderBy(s => s.FTheaterId).
                    Where(s => s.FStartTime.Date.ToString("MM/dd") == realDate && s.FMovieId == movieID);

                List<CShowSession> showSessions = new List<CShowSession>();
                foreach (var sessionItem in sessions)
                {
                    bool verify = false;
                    foreach (var item in showSessions)
                    {
                        if (item.theaterName == sessionItem.FTheater.FTheater)
                        {
                            item.sessionIDandTimeandLeftSeats += $",{sessionItem.FSessionId}##{sessionItem.FStartTime.ToString("HH:mm")}";
                            int leftSeats = showLeftSeats(sessionItem.FSessionId);
                            item.sessionIDandTimeandLeftSeats += $"##{leftSeats}";
                            verify = true;
                            break;
                        }
                    }
                    if (!verify)
                    {
                        CShowSession showSession = new CShowSession();
                        showSession.theaterName = sessionItem.FTheater.FTheater;
                        showSession.sessionIDandTimeandLeftSeats = $"{sessionItem.FSessionId}##{sessionItem.FStartTime.ToString("HH:mm")}";
                        int leftSeats = showLeftSeats(sessionItem.FSessionId);
                        showSession.sessionIDandTimeandLeftSeats += $"##{leftSeats}";
                        showSessions.Add(showSession);
                    }
                }
                return Json(showSessions);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public int showLeftSeats(int sessionID)
        {
            try
            {
                var theater = movieContext.TSessions.Include(t => t.FTheater).FirstOrDefault(s => s.FSessionId == sessionID).FTheater;
                //var seatCount = 400-movieContext.TSeats.Where(s => s.FTheaterId == theater.FTheaterId).Where(s=>s.FSeatStatusId!=2).Count();
                var seatCount = movieContext.TSeats.Where(s => s.FTheaterId == theater.FTheaterId).Where(s => s.FSeatStatusId != 2).ToList();
                var seats = seatCount.Count();
                var soldSeats = movieContext.TOrderDetails.Where(o => o.FOrder.FSessionId == sessionID).Count();
                int leftSeats = seats - soldSeats;
                return leftSeats;
            }
            catch
            {
                return 0;
            }
        }

        public IActionResult SaveSelectedSessionID(int sessionID)
        {
            try
            {
                HttpContext.Session.SetInt32(CDictionary.SelectedSessionID, sessionID);
                return Content("Success");
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }

        }

        public IActionResult checkSessionSelected()
        {
            try
            {
                if (HttpContext.Session.Keys.Contains(CDictionary.SelectedSessionID))
                    return Content("有選到場次");
                else
                    return Content("沒有選到場次");
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult ListTicketClass()
        {
            try
            {
                //判斷會員登入或會員中心
                bool verify_checkLogIn = sessionCheck();

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
                vm.selectedMovieID = movie.FId;
                vm.selectedMovieName = movie.FNameCht;
                vm.selectedMovieEngName = movie.FNameEng;
                vm.selectedMoviePoster = movie.FPosterPath;

                //顯示選的票種價錢
                decimal moviePrice = (decimal)movie.FPrice;
                decimal 早場優惠 = 1;
                if (int.Parse(selectedSessionHour) <= 11)
                    早場優惠 = movieContext.TTicketClasses.FirstOrDefault(t => t.FTicketClassId == 5).FPriceRate;
                decimal 廳加價 = 1;
                if (theater.FTheaterId == 1)
                    廳加價 = movieContext.TTheaterClasses.FirstOrDefault(tc => tc.FTheaterClassId == 2).FPriceRate;
                decimal 一般價 = movieContext.TTicketClasses.FirstOrDefault(t => t.FTicketClassId == 1).FPriceRate;
                vm.oneNormalPrice = Convert.ToInt32(moviePrice * 一般價 * 早場優惠 * 廳加價);
                decimal 學生價 = movieContext.TTicketClasses.FirstOrDefault(t => t.FTicketClassId == 2).FPriceRate;
                vm.oneStudentPrice = Convert.ToInt32(moviePrice * 學生價 * 早場優惠 * 廳加價);
                decimal 軍警價 = movieContext.TTicketClasses.FirstOrDefault(t => t.FTicketClassId == 5).FPriceRate;
                vm.oneSoldierPrice = Convert.ToInt32(moviePrice * 軍警價 * 早場優惠 * 廳加價);

                return View(vm);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult ListSeat(CListSeatViewModel vm)
        {
            try
            {
                //判斷會員登入或會員中心
                bool verify_checkLogIn = sessionCheck();

                int n = vm.sessionID_seat;
                int x = vm.normalCount_seat;
                int y = vm.studentCount_seat;
                int z = vm.soldierCount_seat;

                //將選的票種數量存進session
                string tickets = $"{x},{y},{z}";
                HttpContext.Session.SetString(CDictionary.SelectedTicketClass, tickets);

                //選到的電影
                vm.movieName = movieContext.TSessions.Include(s => s.FMovie).FirstOrDefault(s => s.FSessionId == n).FMovie.FNameCht;
                //選到的日期
                vm.sessionDate = movieContext.TSessions.FirstOrDefault(s => s.FSessionId == n).FStartTime.ToString("MM/dd");
                //選到的時間
                vm.sessionTime = movieContext.TSessions.FirstOrDefault(s => s.FSessionId == n).FStartTime.ToString("HH:mm");
                //全部票數
                vm.totalTickets = (x + y + z).ToString();
                string s = vm.totalTickets;

                //選到的廳
                var theaterID = movieContext.TSessions.FirstOrDefault(s => s.FSessionId == n).FTheaterId;
                vm.theaterID = theaterID.ToString();
                vm.theaterName = movieContext.TTheaters.FirstOrDefault(t => t.FTheaterId == theaterID).FTheater;


                //所有這個場次的seat
                var seats = movieContext.TSessions.Include(s => s.FTheater).ThenInclude(t => t.TSeats).FirstOrDefault(s => s.FSessionId == n).FTheater.TSeats;

                //非座位區的所有ID
                var nonSeats = seats.Where(s => s.FSeatStatusId == 2).Select(s => s.FSeatId);

                //愛心座位的所有ID
                var diabledSeats = seats.Where(s => s.FSeatStatusId == 3).Select(s => s.FSeatId);

                //已被選走的座位
                var selectedSeats = movieContext.TOrderDetails.Include(od => od.FOrder).Where(od => od.FOrder.FSessionId == n).Select(od => od.FSeatId);
                var selectedSeats1 = movieContext.TOrderDetails.Include(od => od.FOrder).Where(od => od.FOrder.FSessionId == n).ToList();


                var orders = movieContext.TOrders.ToList();
                var od = movieContext.TOrderDetails.ToList();
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
                for (int i = 0; i < seat.Length; i++)
                {
                    if (seat[i] == 0)
                        seat[i] = 1;
                }
                vm.seatStatus = seat;

                return View(vm);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult ListProduct()
        {
            try
            {
                //判斷會員登入或會員中心
                bool verify_checkLogIn = sessionCheck();

                var product = movieContext.TProducts.ToList();

                //將產品分門別類
                CListProductViewModel vm = new CListProductViewModel();
                List<CTProductWrap> drinks = new List<CTProductWrap>();
                List<CTProductWrap> popcorns = new List<CTProductWrap>();
                List<CTProductWrap> desserts = new List<CTProductWrap>();
                foreach (var item in product)
                {
                    if (item.FCategoryId == 1)
                    {
                        CTProductWrap productWrap = new CTProductWrap();
                        productWrap.product = movieContext.TProducts.FirstOrDefault(p => p.FProductId == item.FProductId);
                        productWrap.FProductId = movieContext.TProducts.FirstOrDefault(p => p.FProductId == item.FProductId).FProductId;
                        productWrap.FProductName = movieContext.TProducts.FirstOrDefault(p => p.FProductId == item.FProductId).FProductName;
                        productWrap.FProductPrice = movieContext.TProducts.FirstOrDefault(p => p.FProductId == item.FProductId).FProductPrice;
                        productWrap.FImage = movieContext.TProducts.FirstOrDefault(p => p.FProductId == item.FProductId).FImage;
                        drinks.Add(productWrap);
                    }
                    if (item.FCategoryId == 2)
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
                var movie = getSessionMovie(sessionID);
                vm.selectedMovieName = movie.FNameCht;

                //抓時間
                vm.selectedSessionDate = getSessionDate(sessionID);
                vm.selectedSessionTime = getSessionTime(sessionID);

                //抓影廳
                var theater = getTheater(sessionID);
                vm.selectedTheaterID = theater.FTheaterId;
                vm.selectedTheaterName = theater.FTheater;

                //抓人數
                string json_tickets = HttpContext.Session.GetString(CDictionary.SelectedTicketClass);
                string[] tickets = json_tickets.Split(",");

                int totalCount = 0;
                foreach (var item in tickets)
                    totalCount += Convert.ToInt32(item);
                vm.ticketCounts = totalCount.ToString();


                //抓座位
                List<string> showSeats = getSeatNames(theater.FTheaterId);
                vm.selectedSeats = showSeats;

                return View(vm);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }

        }

        public IActionResult saveSeatIDinSession(int correctSeatID, int totalTickets, string selected)
        {
            try
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
                if (selected == "1")
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
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult showSelectedSeat()
        {
            try
            {
                List<string> seatNames = getSeatNames(getTheater(getSessionID()).FTheaterId);
                return Json(seatNames);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public void saveProductCount(List<CProductInfo> productInfos)
        {
            string json = System.Text.Json.JsonSerializer.Serialize(productInfos);
            HttpContext.Session.SetString(CDictionary.SelectedProductCount, json);
        }



        public IActionResult ListOrderDetails(List<CProductInfo> products)
        {
            try
            {
                if (products.Count != 0)
                    saveProductCount(products);

                //確認有沒有登入會員，if沒有，先登入會員
                string controller = "OrderFront";
                string view = "ListOrderDetails";
                HttpContext.Session.SetString(CDictionary.SK_CONTROLLER, controller);
                HttpContext.Session.SetString(CDictionary.SK_VIEW, view);
                bool verify_checkLogIn = sessionCheck();
                if (!verify_checkLogIn)
                    return RedirectToAction("memberLogIn", "memberFront");


                CListOrderDetailsViewModel vm = new CListOrderDetailsViewModel();

                //選擇到的票種張數
                string json_tickets = HttpContext.Session.GetString(CDictionary.SelectedTicketClass);
                string[] tickets = json_tickets.Split(",");
                string[] ticketsNames = new string[] { "一般票", "學生票", "軍警票" };
                string selectedTickets = "";
                for (int i = 0; i < tickets.Length; i++)
                {
                    if (Convert.ToInt32(tickets[i]) > 0)
                        selectedTickets += $"{ticketsNames[i]}:{tickets[i]}張,";
                }
                if (selectedTickets.Trim().Substring(selectedTickets.Trim().Length - 1, 1) == ",")
                    selectedTickets = selectedTickets.Trim().Substring(0, selectedTickets.Trim().Length - 1);
                vm.tickets = selectedTickets;

                //選擇到的電影名稱、日期時間
                int sessionID = getSessionID();
                var movie = getSessionMovie(sessionID);
                vm.selectedMovieName = movie.FNameCht;
                vm.selectedMoviEngeName = movie.FNameEng;
                vm.selectedMoviePoster = movie.FPosterPath;
                vm.selectedMovieID = movie.FId;
                var selectedSessionHour = getSessionHour(sessionID);
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
                List<CProductInfo> productCounts = getProductCounts();
                List<string> showProducts = new List<string>();
                foreach (var item in productCounts)
                {
                    if (int.Parse(item.productCount) > 0)
                    {
                        var selectedProduct = movieContext.TProducts.FirstOrDefault(p => p.FProductId == item.productID);
                        string productName = "";
                        if (selectedProduct.FCategoryId == 2)
                            productName = selectedProduct.FProductName + "爆米花";
                        else
                            productName = selectedProduct.FProductName;
                        string thisProduct = $"{productName}X{item.productCount}";
                        showProducts.Add(thisProduct);
                    }
                }
                vm.set = showProducts;

                //擁有的優惠券
                int memID = getMemberID();
                var couponList = movieContext.TCouponLists.Include(c => c.FCouponType).Where(c => c.FMemberId == memID
                    & c.FCouponType.FCouponDueDate.Month == DateTime.Now.Month & c.FIsUsed == false)
                    .OrderByDescending(c => c.FCouponType.FCouponDueDate).ToList();
                vm.CouponList = couponList;

                //總價
                int ticketFee = 0;
                decimal 電影基本價 = (decimal)movie.FPrice;
                int 一般票張數 = Convert.ToInt32(tickets[0]);
                int 學生票張數 = Convert.ToInt32(tickets[1]);
                int 軍警票張數 = Convert.ToInt32(tickets[2]);
                decimal 早場優惠 = 1;
                if (int.Parse(selectedSessionHour) <= 11)
                    早場優惠 = movieContext.TTicketClasses.FirstOrDefault(t => t.FTicketClassId == 5).FPriceRate;
                decimal 廳加價 = 1;
                if (theater.FTheaterId == 1)
                    廳加價 = movieContext.TTheaterClasses.FirstOrDefault(tc => tc.FTheaterClassId == 2).FPriceRate;
                decimal 會員優惠 = (decimal)movieContext.TMembers.Include(m => m.FMembership).FirstOrDefault(m => m.FMemberId == memID).FMembership.FPriceRate;

                if (一般票張數 > 0)
                {
                    decimal 一般價 = movieContext.TTicketClasses.FirstOrDefault(t => t.FTicketClassId == 1).FPriceRate;
                    ticketFee += Convert.ToInt32(電影基本價 * 一般價 * 早場優惠 * 廳加價 * 一般票張數 * 會員優惠);
                }
                if (學生票張數 > 0)
                {
                    decimal 學生價 = movieContext.TTicketClasses.FirstOrDefault(t => t.FTicketClassId == 2).FPriceRate;
                    ticketFee += Convert.ToInt32(電影基本價 * 學生價 * 早場優惠 * 廳加價 * 學生票張數 * 會員優惠);
                }
                if (軍警票張數 > 0)
                {
                    decimal 軍警價 = movieContext.TTicketClasses.FirstOrDefault(t => t.FTicketClassId == 5).FPriceRate;
                    ticketFee += Convert.ToInt32(電影基本價 * 軍警價 * 早場優惠 * 廳加價 * 學生票張數 * 會員優惠);
                }

                //產品總價
                int productFee = 0;
                for (int i = 0; i < productCounts.Count; i++)
                {
                    var selectedProduct = movieContext.TProducts.
                        FirstOrDefault(p => p.FProductId == productCounts[i].productID);
                    productFee += selectedProduct.FProductPrice * Convert.ToInt32(productCounts[i].productCount);
                }

                vm.totalPrice = (ticketFee + productFee).ToString();

                return View(vm);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult SaveOrdertoDBandSendEmail(int totalPrice, int? couponID, string paymentType)
        {
            try
            {
                using (var tran = movieContext.Database.BeginTransaction())
                {
                    //新增order資料
                    TOrder order = new TOrder();
                    int sessionID = getSessionID();
                    order.FSessionId = sessionID;
                    int memID = getMemberID();
                    order.FMemberId = memID;
                    if (paymentType == "cash")
                        order.FPayTypeId = 1;//現金付款
                    else
                        order.FPayTypeId = 2;//信用卡付款
                    order.FOrderDate = DateTime.Now;
                    if (couponID != null)
                        order.FCouponId = couponID;
                    order.FTotalPrice = totalPrice;

                    movieContext.TOrders.Add(order);
                    movieContext.SaveChanges();

                    //用掉的優惠券
                    if (couponID != null)
                    {
                        var usedCoupon = movieContext.TCouponLists.FirstOrDefault(c => c.FCouponId == couponID);
                        usedCoupon.FIsUsed = true;
                        movieContext.SaveChanges();
                    }

                    //新增orderDetails資料
                    //查總共票數(座位)
                    string json_tickets = HttpContext.Session.GetString(CDictionary.SelectedTicketClass);
                    string[] tickets = json_tickets.Split(",");
                    int[] ticketCounts = new int[tickets.Length];
                    for (int i = 0; i < tickets.Length; i++)
                        ticketCounts[i] = int.Parse(tickets[i]);

                    //將票種轉陣列
                    int[] ticketSelected = new int[(ticketCounts[0] + ticketCounts[1] + ticketCounts[2])];
                    for (int i = 0; i < ticketCounts[0]; i++)
                        ticketSelected[i] = 1;
                    for (int i = 0; i < ticketCounts[1]; i++)
                        ticketSelected[i + ticketCounts[0]] = 2;
                    for (int i = 0; i < ticketCounts[2]; i++)
                        ticketSelected[i + ticketCounts[0] + ticketCounts[1]] = 5;

                    string json_seats = HttpContext.Session.GetString(CDictionary.SelectedSeatID);
                    List<int> seats = System.Text.Json.JsonSerializer.Deserialize<List<int>>(json_seats);
                    int orderID = movieContext.TOrders.OrderByDescending(od => od.FOrderId).Select(od => od.FOrderId).FirstOrDefault();
                    for (int i = 0; i < seats.Count; i++)
                    {
                        TOrderDetail orderDetail = new TOrderDetail();
                        orderDetail.FOrderId = orderID;
                        orderDetail.FSeatId = seats[i];
                        orderDetail.FTicketClassId = ticketSelected[i];
                        movieContext.TOrderDetails.Add(orderDetail);
                        movieContext.SaveChanges();
                    }

                    //新增orderStatus資料
                    TOrderStatus orderStatus = new TOrderStatus();
                    orderStatus.FOrderStatus = "未取票";
                    orderStatus.FChangedTime = DateTime.Now;
                    movieContext.TOrderStatuses.Add(orderStatus);
                    movieContext.SaveChanges();

                    //新增orderStatusLog資料
                    TOrderStatusLog orderStatusLog = new TOrderStatusLog();
                    orderStatusLog.FOrderId = orderID;
                    int orderStatusID = movieContext.TOrderStatuses.OrderByDescending(od => od.FOrderStatusId).Select(od => od.FOrderStatusId).FirstOrDefault();
                    orderStatusLog.FOrderStatusId = orderStatusID;
                    movieContext.TOrderStatusLogs.Add(orderStatusLog);
                    movieContext.SaveChanges();

                    //新增產品收據資料
                    List<CProductInfo> productInfo = getProductCounts();
                    bool buyProducts = false;
                    foreach (var item in productInfo)
                    {
                        if (Convert.ToInt32(item.productCount) > 0)
                        {
                            buyProducts = true;
                            break;
                        }
                    }

                    if (buyProducts)
                    {
                        TReceipt receipt = new TReceipt();
                        receipt.FMemberId = memID;
                        receipt.FReceiptDate = DateTime.Now;
                        receipt.FOrderId = orderID;
                        movieContext.TReceipts.Add(receipt);
                        movieContext.SaveChanges();
                    }

                    //新增ReceiptDetails
                    var receiptID = movieContext.TReceipts.OrderByDescending(r => r.FReceiptId).Select(r => r.FReceiptId).FirstOrDefault();
                    for (int i = 0; i < productInfo.Count; i++)
                    {
                        if (Convert.ToInt32(productInfo[i].productCount) > 0)
                        {
                            TReceiptDetail receiptDetail = new TReceiptDetail();
                            receiptDetail.FReceiptId = receiptID;
                            receiptDetail.FProductId = productInfo[i].productID;
                            receiptDetail.FQty = Convert.ToInt32(productInfo[i].productCount);
                            movieContext.TReceiptDetails.Add(receiptDetail);
                            movieContext.SaveChanges();
                        }
                    }

                    SendEmail();
                    ClearAllSession();
                    tran.Commit();
                    return Content("儲存成功");
                }
            }
            catch (Exception)
            {
                return Content("交易失敗");
            }

        }

        public IActionResult ListOrderBook(string orderStatusDescribe)
        {
            try
            {
                int memberID = getMemberID();

                var orderStatus_取票狀態 = movieContext.TOrderStatuses.Where(od => od.FOrderStatus == orderStatusDescribe).
                    OrderByDescending(o => o.FOrderStatusId).
                    Select(od => od.FOrderStatusId).ToList();
                List<TOrderStatusLog> order_取票狀態 = new List<TOrderStatusLog>();
                foreach (var item in orderStatus_取票狀態)
                    order_取票狀態.Add(movieContext.TOrderStatusLogs.
                        FirstOrDefault(osl => osl.FOrderStatusId == item));
                List<TOrder> this會員的取票 = new List<TOrder>();
                foreach (var item in order_取票狀態)
                {
                    var mightNull取票狀態 = movieContext.TOrders.Include(o => o.FSession).
                        ThenInclude(s => s.FTheater).
                        Include(o => o.FSession).ThenInclude(s => s.FMovie).
                        FirstOrDefault(o => o.FMemberId == memberID & o.FOrderId == item.FOrderId);
                    if (mightNull取票狀態 != null)
                        this會員的取票.Add(mightNull取票狀態);
                }

                List<CticketData> ticketDatas = new List<CticketData>();
                foreach (var item in this會員的取票)
                {
                    CticketData data = new CticketData();
                    data.orderID = item.FOrderId;
                    data.theaterName = item.FSession.FTheater.FTheater;
                    data.movieName = item.FSession.FMovie.FNameCht;
                    data.sessionStartTime = item.FSession.FStartTime.ToString("MM/dd HH:mm");
                    data.totalPrice = (int)item.FTotalPrice;
                    ticketDatas.Add(data);
                }

                return Json(ticketDatas);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult ShowOrderDetails(int orderID)
        {
            try
            {
                var orderDetails = movieContext.TOrderDetails.Where(od => od.FOrderId == orderID).Select(od => new { od.FSeatId, od.FTicketClass.FTicketClass }).ToList();
                int theaterID = movieContext.TOrders.Include(o => o.FSession).FirstOrDefault(o => o.FOrderId == orderID).FSession.FTheaterId;

                List<string> listSeatNames = new List<string>();
                foreach (var item in orderDetails)
                {
                    int row;
                    int col;
                    int seatIDinTheater = item.FSeatId - (theaterID - 1) * 400;
                    if ((seatIDinTheater) % 20 != 0)
                    {
                        row = (seatIDinTheater) / 20 + 1;
                        col = (seatIDinTheater) % 20;
                    }
                    else
                    {
                        row = (seatIDinTheater) / 20;
                        col = 20;
                    }
                    listSeatNames.Add($"{row}排{col}");
                }

                List<COrderDetailData> showOrderDetail = new List<COrderDetailData>();
                for (int i = 0; i < listSeatNames.Count; i++)
                {
                    COrderDetailData data = new COrderDetailData();
                    data.seatName = listSeatNames[i];
                    data.ticketClassName = orderDetails[i].FTicketClass;
                    showOrderDetail.Add(data);
                }
                return Json(showOrderDetail);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult ShowReceiptDetails(int orderID)
        {
            try
            {
                var receipt = movieContext.TReceipts.FirstOrDefault(r => r.FOrderId == orderID);
                List<TReceiptDetail> receiptDetails = new List<TReceiptDetail>();
                if (receipt != null)
                {
                    int receiptID = receipt.FReceiptId;
                    receiptDetails = movieContext.TReceiptDetails.Include(rd => rd.FProduct).
                        Where(rd => rd.FReceiptId == receiptID).ToList();
                }

                List<CReceiptDetailData> list = new List<CReceiptDetailData>();
                foreach (var item in receiptDetails)
                {
                    CReceiptDetailData receiptDetailData = new CReceiptDetailData();
                    if (item.FProduct.FCategoryId == 2)
                        receiptDetailData.productName = movieContext.TProducts.FirstOrDefault(p => p.FProductId == item.FProductId).FProductName + "爆米花";
                    else
                        receiptDetailData.productName = movieContext.TProducts.FirstOrDefault(p => p.FProductId == item.FProductId).FProductName;
                    receiptDetailData.productCount = item.FQty.ToString();
                    list.Add(receiptDetailData);
                }
                return Json(list);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }

        }
        public IActionResult deleteOrder(int orderID)
        {
            try
            {
                var deleted = movieContext.TOrderDetails.Where(od => od.FOrderId == orderID);
                foreach (var item in deleted)
                    movieContext.TOrderDetails.Remove(item);

                var orderStatusLog = movieContext.TOrderStatusLogs.FirstOrDefault(osl => osl.FOrderId == orderID);
                var orderStatus = movieContext.TOrderStatuses.FirstOrDefault(os => os.FOrderStatusId == orderStatusLog.FOrderStatusId);
                orderStatus.FOrderStatus = "已取消";

                movieContext.SaveChanges();
                return Content("Success");
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult SearchMovieKeyword(string keyword)
        {
            try
            {
                var result = movieContext.TSessions.Include(s => s.FMovie).AsEnumerable().
               Where(s => s.FStartTime.Date > DateTime.Now.Date).
               Where(s => s.FMovie.FNameCht.Contains(keyword)).
               DistinctBy(s => s.FMovieId).ToList();

                return Json(result);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [NonAction]
        string getNames(Array data)
        {
            string result = "";
            foreach (var item in data)
            {
                result += " " + item;
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
            var movie = movieContext.TSessions.Include(s => s.FMovie).FirstOrDefault(s => s.FSessionId == sessionID).FMovie;
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
            foreach (var item in seats)
            {
                int row;
                int col;
                int seatIDinTheater = item - (theaterID - 1) * 400;
                if ((seatIDinTheater) % 20 != 0)
                {
                    row = (seatIDinTheater) / 20 + 1;
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

        int getMemberID()
        {
            return (int)HttpContext.Session.GetInt32(CDictionary.SK_LOGIN_USER); ;
        }

        List<CProductInfo> getProductCounts()
        {
            string json_set = HttpContext.Session.GetString(CDictionary.SelectedProductCount);
            List<CProductInfo> product = System.Text.Json.JsonSerializer.Deserialize<List<CProductInfo>>(json_set);
            return product;
        }

        public bool sessionCheck()
        {
            var userId = HttpContext.Session.GetInt32(CDictionary.SK_LOGIN_USER);
            var userName = HttpContext.Session.GetString(CDictionary.SK_LOGIN_USER_NAME);
            bool isUserLoggedIn = userId != null;
            ViewBag.Login = isUserLoggedIn;
            ViewBag.UserId = userId;
            ViewBag.userName = userName;
            return isUserLoggedIn;
        }


        /*Bitmap CreateQRCode(string content, int width, int height)
        {
            //var writer = new BarcodeWriter();
            var writer = new BarcodeWriterPixelData();
            writer.Format = BarcodeFormat.QR_CODE;
            QrCodeEncodingOptions options = new QrCodeEncodingOptions();
            options.Height = height;
            options.Width = width;
            options.Margin = 1;
            options.CharacterSet = "UTF-8";
            options.ErrorCorrection = ZXing.QrCode.Internal.ErrorCorrectionLevel.M;
            options.DisableECI = true;
            Bitmap QRCode = writer.Write(content);

            string attimg = Environment.CurrentDirectory + @"\qr.png";
            //QRCode.Save(attimg);
           
            return QRCode;
        }*/

        void SendEmail()
        {
            string GoogleID = "iSpanMovieTheater@gmail.com";
            string TempPassword = "yrxrcgvlwhtufszp";
            string ReceiveEmail = "zhuo.demo222@gmail.com";

            string SmtpServer = "smtp.gmail.com";
            int SmtpPort = 587;

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(GoogleID, "MovieHolic");
            mail.Subject = "您已成功訂購電影票";

            //信件內容
            int memID = getMemberID();
            int sessionID = getSessionID();
            string memName = movieContext.TMembers.FirstOrDefault(m => m.FMemberId == memID).FName;
            string movieName = getSessionMovie(sessionID).FNameCht;
            string movieTime = getSessionDate(sessionID) + "  " + getSessionTime(sessionID);
            List<string> seatNames = getSeatNames(getTheater(sessionID).FTheaterId);
            string seats = "";
            foreach (var item in seatNames)
                seats += item + "、";
            if (seats.Substring(seats.Length - 1, 1) == "、")
                seats = seats.Substring(0, seats.Length - 1);

            string bodyContent = $"<p>親愛的 {memName}：<br><br>非常感謝您選擇我們的MovieHolic觀賞電影！" +
                $"我們已收到您的訂購並確認您的票價和座位資訊。以下是您的訂購詳細信息：<br><br>" +
                $"電影名稱：{movieName}<br>放映時間：{movieTime}<br>座位號碼：{seats}<br>" +
                $"請在放映前提前到達電影院，您可以使用此郵件作為入場證明。前15分鐘到達，以確保有足夠的時間順利入場。<br>" +
                $"如有任何疑問或需要進一步協助，請隨時與我們的客服部門聯繫，他們將樂意為您提供幫助。<br>" +
                $"再次感謝您選擇我們的電影院，我們期待為您提供一次愉快的觀影體驗！<br>祝您有美好的觀影時光！<br><br>" +
                $"誠摯的祝福，<br>MovieHolic 瘋電影</p>  <img src=\"cid:fs_logo01.png\">";
            mail.Body = bodyContent;


            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(bodyContent, null, "text/html");
            LinkedResource res = new LinkedResource("wwwroot/images/fs_logo01.png", "image/png");
            res.ContentId = "fs_logo01";
            alternateView.LinkedResources.Add(res);
            mail.AlternateViews.Add(alternateView);


            mail.IsBodyHtml = true;
            mail.SubjectEncoding = Encoding.UTF8;
            mail.To.Add(new MailAddress(ReceiveEmail));

            using (SmtpClient client = new SmtpClient(SmtpServer, SmtpPort))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(GoogleID, TempPassword);
                client.Send(mail);
            }
        }

        void ClearAllSession()
        {
            HttpContext.Session.Remove(CDictionary.SelectedSessionID);
            HttpContext.Session.Remove(CDictionary.SelectedTicketClass);
            HttpContext.Session.Remove(CDictionary.SelectedSeatID);
            HttpContext.Session.Remove(CDictionary.SelectedProductCount);
        }
    }

    public class CShowSession
    {
        public string theaterName { get; set; }
        public string sessionIDandTimeandLeftSeats { get; set; }
    }

    public class CticketData
    {
        public int orderID { get; set; }
        public string theaterName { get; set; }
        public string movieName { get; set; }
        public string sessionStartTime { get; set; }
        public int totalPrice { get; set; }
    }

    public class COrderDetailData
    {
        public string seatName { get; set; }
        public string ticketClassName { get; set; }
    }

    public class CReceiptDetailData
    {
        public string productName { get; set; }
        public string productCount { get; set; }
    }

}
