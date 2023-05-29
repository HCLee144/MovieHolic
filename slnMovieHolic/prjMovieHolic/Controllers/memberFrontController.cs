using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjMovieHolic.Models;
using prjMovieHolic.Models.Member;
using prjMovieHolic.ViewModels;
using System.Drawing.Drawing2D;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace prjMovieHolic.Controllers
{
    public class memberFrontController : SuperFrontController
    {
        private readonly MovieContext _movieContext;
        public memberFrontController(MovieContext context)
        {
            _movieContext= context;
        }
        //會員登入
        public IActionResult memberLogin()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult memberLogin(CMemberViewModel vm)
        {
            TMember user=_movieContext.TMembers.FirstOrDefault(t=>t.FPhone.Equals(vm.txtAccount));
            //bool verifyPassword = CPasswordHasher.VerifyPassword(vm.txtPassword, user.FPassword);
            if (user != null && user.FPassword.Equals(vm.txtPassword))
            {
                //string json=JsonSerializer.Serialize(user.);
                HttpContext.Session.SetInt32(CDictionary.SK_LOGIN_USER,user.FMemberId);
                HttpContext.Session.SetString(CDictionary.SK_LOGIN_USER_NAME, user.FName);
                ViewBag.Login = true;
                ViewBag.userId = user.FMemberId;
                ViewBag.userName=user.FName;
                string controller=HttpContext.Session.GetString(CDictionary.SK_CONTROLLER);
                string view = HttpContext.Session.GetString(CDictionary.SK_VIEW);
                int? parameter = HttpContext.Session.GetInt32(CDictionary.SK_PARAMETER);
                ViewBag.Controller = controller;
                ViewBag.View = view;
                if (controller != null && view != null && parameter !=null)
                    return RedirectToAction(view, controller, new {id=parameter});
                else if(controller != null && view != null && parameter == null)
                    return RedirectToAction(view, controller);
                else
                    return RedirectToAction("memberList", "memberFront", new {id=vm.FMemberId});
               
            }
            return View();
        }
        //登入：驗證是否成功登入
        public IActionResult IsLogin(string txtAccount,string txtPassword)
        {
            bool isSent = _movieContext.TMembers.Where(t => t.FPhone==txtAccount && t.FPassword==txtPassword).ToList().Any();
            if (!isSent)
            {
                return Json(new { success = false, message = "您輸入的帳號或密碼錯誤，請重新輸入。" });
            }
            else
            {
                return Json(new { success = true, message = "" });
            }

        }
        //登出
        public IActionResult memberLogout()
        {
            HttpContext.Session.Remove(CDictionary.SK_LOGIN_USER);
            HttpContext.Session.Remove(CDictionary.SK_LOGIN_USER_NAME);
            HttpContext.Session.Remove(CDictionary.SK_CONTROLLER);
            HttpContext.Session.Remove(CDictionary.SK_VIEW);
            HttpContext.Session.Remove(CDictionary.SK_PARAMETER);
            return RedirectToAction("Index","Home");
        }
        //todo 尚未驗證完成 註冊會員
        public IActionResult memberSignUp()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult memberSignUp(TMember member)
        {
            try
            {
                var accountCheck = _movieContext.TMembers.Any(t => t.FPhone == member.FPhone);
                if (accountCheck == false)
                {
                    _movieContext.TMembers.Add(member);
                    //_movieContext.SaveChanges();
                    //TCouponList couponList = new TCouponList();
                    //couponList.FCouponTypeId = (int)DateTime.Now.Month;
                    //couponList.FMemberId = member.FMemberId;
                    //couponList.FIsUsed = false;
                    //couponList.FReceiveDate = DateTime.Now;
                    //couponList.FOrderId = null;
                    //_movieContext.TCouponLists.Add(couponList);
                    _movieContext.SaveChanges();
                    return Content("註冊成功");
                }
                return Content("註冊失敗");
            }
            catch(Exception ex)
            {
                return Content(null);
            }
            
         
        }
        //註冊：驗證帳號是否已存在
        public IActionResult accountCheck(string FPhone)
        {
            var accountCheck=_movieContext.TMembers.Any(t=>t.FPhone==FPhone);
            return Content(accountCheck.ToString());
        }
        //註冊：回傳是否註冊成功
        public IActionResult IsSignUp(CSignUpViewModel vm)
        {
            //if (ModelState.IsValid)
            //{
            var accountCheck = _movieContext.TMembers.Any(t => t.FPhone == vm.FPhone);
            bool passwordFormat = !string.IsNullOrEmpty(vm.FPassword) && Regex.IsMatch(vm.FPassword, @"(?=.{8,16})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])");
            bool passwordDoubleCheck = false;
            if (vm.FPassword != null)
            {
                passwordDoubleCheck = vm.FPassword.Equals(vm.FPasswordCheck);
            }

            if (accountCheck != true && passwordFormat == true && passwordDoubleCheck == true)
            {
                return Json(new { success = true, message = "註冊成功。" });
            }
            else
            {
                return Json(new { success = false, message = "註冊失敗，請重新註冊。" });
            }
            //}
            //return View();
        }



        //忘記密碼
        public IActionResult forgetPassword()
        {   //step1 驗證是否有此帳號
            //step2 寄信
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult forgetPassword(CMemberViewModel vm)
        {
            string email = vm.txtForgetPasswordEmail;
            bool userExists = _movieContext.TMembers.Where(o => o.FEmail == email).ToList().Any();
            if (!userExists)
            {
                var message = new { text = "該電子郵件地址不存在。", type = "false" };
                return View("forgetPassword",message);
            }
            else
            { 
                CForgetPassword CforgetPassword = new CForgetPassword();
                CforgetPassword.getNewPasswordEmail(email);
                return RedirectToAction("memberLogin");
            }
        }
        //忘記密碼：驗證是否有此會員
        public IActionResult IsForgetPassword(string forgetPasswordEmail)
        {
            bool isSent = _movieContext.TMembers.Any(o => o.FEmail == forgetPasswordEmail);
            if (!isSent)
            {
                return Json(new { success = false, message = "該電子郵件地址不存在。" });
            }
            else
            {
                return Json(new { success = true, message = "已成功寄出新密碼信件，請至您的信箱收取新密碼。" });
            }
        }

        //會員首頁
        public IActionResult memberIndex(int? id) 
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_USER))
            {
                TMember memberData = _movieContext.TMembers.Include(t => t.FMembership)
                    .Include(t => t.FGender)
                    .FirstOrDefault(t => t.FMemberId == id);

                sessionCheck();

                CMemberAndOtherViewModel viewModel = new CMemberAndOtherViewModel()
                {
                    Member = memberData,
                    CouponList = queryCoupon(id),
                    MemberActionNow = queryFavorite(id),
                    TotalPrice = queryTotalPrice(id),
                };

                return View(viewModel);
            }
            return RedirectToAction("memberLogin");
            
        }   


        //會員基本資料
        public IActionResult memberList(int? id)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_USER))
            {
            TMember memberData = _movieContext.TMembers.Include(t => t.FMembership)
                .Include(t => t.FGender)
                .FirstOrDefault(t => t.FMemberId == id);

                sessionCheck();

                CMemberAndOtherViewModel viewModel = new CMemberAndOtherViewModel()
                {
                    Member = memberData,
                    CouponList = queryCoupon(id),
                    MemberActionNow = queryFavorite(id),
                };

                return View(viewModel);
            }
            return RedirectToAction("memberLogin");
        }
        //修改會員資料
        public IActionResult memberEdit(int? id)
        {
            TMember memberData = _movieContext.TMembers.Include(t => t.FMembership)
                .Include(t => t.FGender)
                .FirstOrDefault(t => t.FMemberId == id);
            sessionCheck();
            if (memberData != null)
            {   
                CMemberAndOtherViewModel viewModel = new CMemberAndOtherViewModel()
                {
                    Member= memberData,
                    CouponList= queryCoupon(id),
                    MemberActionNow=queryFavorite(id),
                };
                return View(viewModel);
            }
            else
                return RedirectToAction("memberList",new {id=id});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult memberEdit(TMember member)
        {

            var memberData = _movieContext.TMembers.Include(t => t.FMembership)
                .Include(t => t.FGender)
                .FirstOrDefault(t => t.FMemberId == member.FMemberId);
            if (memberData != null)
            {
                memberData.FName = member.FName;
                memberData.FNickname= member.FNickname;
                memberData.FBirthDate = member.FBirthDate;
                memberData.FIdcardNumber = member.FIdcardNumber;
                memberData.FGenderId = member.FGenderId;
                memberData.FEmail = member.FEmail;
                _movieContext.SaveChanges();
                
            }
            return RedirectToAction("memberList", new {id=member.FMemberId});
        }

        //密碼修改
        public IActionResult passwordEdit(int? id)
        {
            TMember memberData = _movieContext.TMembers.Include(t => t.FMembership)
                               .Include(t => t.FGender)
                               .FirstOrDefault(t => t.FMemberId == id);
            CMemberAndOtherViewModel viewModel = new CMemberAndOtherViewModel()
            {
                Member = memberData,
                CouponList = queryCoupon(id),
                MemberActionNow = queryFavorite(id),
            };
            sessionCheck();
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult passwordEdit(CMemberViewModel vm)
        { 
            var memberData = _movieContext.TMembers.FirstOrDefault(t=>t.FMemberId==vm.FMemberId);
            //bool verifyPassword = CPasswordHasher.VerifyPassword(vm.txtPassword, memberData.FPassword);
            if (memberData != null)
            {
                var password = _movieContext.TMembers.FirstOrDefault(t => t.FPassword == vm.txtPreviousFPassword);
                bool passwordFormat = !string.IsNullOrEmpty(vm.txtNewFPassword) && Regex.IsMatch(vm.txtNewFPassword, @"(?=.{8,16})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])");
                bool passwordDoubleCheck = vm.txtNewFPassword.Equals(vm.txtNewFPasswordCheck);
                if (password != null && passwordFormat==true && passwordDoubleCheck==true)
                {
                    //string newPassword=CPasswordHasher.HashPassword(vm.txtNewFPasswordCheck);
                    memberData.FPassword = vm.txtNewFPasswordCheck;
                    _movieContext.SaveChanges();
                    return RedirectToAction("memberList", new { id = vm.FMemberId });
                }
                return View(memberData);
            }
            return View(memberData);
        }
        //舊密碼確認
        public IActionResult passwordCheck(string previousPassword)
        {
            var password = _movieContext.TMembers.Any(t => t.FPassword.Equals(previousPassword));
            return Content(password.ToString());
        }
        //密碼格式驗證
        public IActionResult passwordFormat(string newPassword)
        {
            bool passwordFormat= !string.IsNullOrEmpty(newPassword) && Regex.IsMatch(newPassword, @"(?=.{8,16})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])");
            return Content(passwordFormat.ToString());
        }
        //新密碼Double Check
        public IActionResult passwordDoubleCheck(string newPassword,string newPasswordCheck)
        {
            bool doubleCheck= newPasswordCheck?.Equals(newPassword) ?? false;
            return Content(doubleCheck.ToString());
        }
        //密碼修改：回傳密碼是否修改成功
        public IActionResult IsPasswordEdit(string previousPassword,string newPassword, string newPasswordCheck)
        {
            var password = _movieContext.TMembers.FirstOrDefault(t => t.FPassword == previousPassword);
            bool passwordFormat = !string.IsNullOrEmpty(newPassword) && Regex.IsMatch(newPassword, @"(?=.{8,16})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])");
            bool passwordDoubleCheck = newPassword.Equals(newPasswordCheck);
            if (password != null && passwordFormat == true && passwordDoubleCheck == true)
            {
                return Json(new { success = true, message = "修改密碼成功。" });
            }
            return Json(new { success = false, message = "密碼修改失敗，請重新輸入。" });
        }
        //優惠卷顯示
        public IActionResult couponList(int? id)
        {
			var couponList = _movieContext.TCouponLists.Include(c=>c.FCouponType).Where(c=>c.FMemberId==id & c.FCouponType.FCouponDueDate.Month==DateTime.Now.Month & c.FIsUsed==false)
                .OrderByDescending(c=>c.FCouponType.FCouponDueDate).ToList();
            var couponListExpired = _movieContext.TCouponLists.Include(c => c.FCouponType).Where(c => c.FMemberId == id & c.FCouponType.FCouponDueDate.Month < DateTime.Now.Month & c.FIsUsed==false)
                .OrderByDescending(c => c.FCouponType.FCouponDueDate).ToList();
            var couponListUsed= _movieContext.TCouponLists.Include(c => c.FCouponType).Where(c => c.FMemberId == id & c.FIsUsed==true)
				.OrderByDescending(c => c.FCouponType.FCouponDueDate).ToList();
			var members = _movieContext.TMembers.FirstOrDefault(c=>c.FMemberId==id);
            var movie = _movieContext.TMovies.FirstOrDefault(c => c.FId == 1);

            var viewModel = new CCouponAndMemberViewModel
            {
                CouponList = couponList,
                CouponListExpired = couponListExpired,
                CouponListUsed = couponListUsed,
                Member = members,
                Movie = movie,
                MemberActionNow = queryFavorite(id),
			};
            sessionCheck();
			return View(viewModel);
        }
        //收藏頁面顯示
        public IActionResult favoriteList(int? id)
        {
            var members=_movieContext.TMembers.FirstOrDefault(c=>c.FMemberId==id);
            var memberActionNow=_movieContext.TMemberActions.Include(c=>c.FMovie).ThenInclude(c=>c.TSessions)
                .Where(c=>c.FMemberId==id & c.FMovie.FScheduleStart < DateTime.Now & c.FMovie.FScheduleEnd > DateTime.Now & c.FActionTypeId==1).OrderByDescending(c=>c.FTimeStamp).ToList();
            var memberActionFuture= _movieContext.TMemberActions.Include(c => c.FMovie).ThenInclude(c => c.TSessions)
                .Where(c => c.FMemberId == id & c.FMovie.FScheduleStart > DateTime.Now & c.FActionTypeId==1) .ToList();
            var memberActionExpired = _movieContext.TMemberActions.Include(c => c.FMovie).ThenInclude(c => c.TSessions)
                .Where(c => c.FMemberId == id & c.FMovie.FScheduleEnd < DateTime.Now & c.FActionTypeId==1).ToList();
            var viewModel = new CMovieAndMemberViewModel
            {
                Member = members,
                MemberActionNow = memberActionNow,
                MemberActionFuture = memberActionFuture,
                MemberActionExpired = memberActionExpired,
                CouponList= queryCoupon(id),

            };
            sessionCheck();
            return View(viewModel);
        }
        //取消收藏
        public IActionResult favoriteCancel(int FMemberId, int FMovieId)
        {
            TMemberAction favorite = _movieContext.TMemberActions
                .FirstOrDefault(m => m.FMemberId == FMemberId & m.FMovieId == FMovieId);

            if (favorite != null)
            {
                favorite.FActionTypeId = 2;
                favorite.FTimeStamp= DateTime.Now;
                _movieContext.SaveChanges();
            }
            return Content("取消收藏");
        }

      


        //訂單查詢
        public IActionResult orderList()
        {
            int id=(int)HttpContext.Session.GetInt32(CDictionary.SK_LOGIN_USER); 

            var members = _movieContext.TMembers.FirstOrDefault(c => c.FMemberId == id);
            

            //var orderStatus_未取票 = _movieContext.TOrderStatuses.Where(od => od.FOrderStatus == "未取票").Select(od => od.FOrderStatusId).ToList();
            //List<TOrderStatusLog> order_未取票 = new List<TOrderStatusLog>();
            //foreach(var item in orderStatus_未取票)
            //    order_未取票.Add(_movieContext.TOrderStatusLogs.FirstOrDefault(osl => osl.FOrderStatusId == item));
            //List<TOrder> this會員的未取票 = new List<TOrder>();
            //foreach (var item in order_未取票)
            //{
            //    var mightNull未取票 = _movieContext.TOrders.Include(o => o.FSession).ThenInclude(s => s.FTheater).
            //        Include(o => o.FSession).ThenInclude(s => s.FMovie).
            //        FirstOrDefault(o => o.FMemberId == members.FMemberId & o.FOrderId == item.FOrderId);
            //    if(mightNull未取票!=null)
            //    this會員的未取票.Add(mightNull未取票);
            //}

            //var orderStatus_已取票 = _movieContext.TOrderStatuses.Where(od => od.FOrderStatus == "已取票").Select(od => od.FOrderStatusId).ToList();
            //List<TOrderStatusLog> order_已取票 = new List<TOrderStatusLog>();
            //foreach (var item in orderStatus_已取票)
            //    order_已取票.Add(_movieContext.TOrderStatusLogs.FirstOrDefault(osl => osl.FOrderStatusId == item));
            //List<TOrder> this會員的已取票 = new List<TOrder>();
            //foreach (var item in order_已取票)
            //{
            //    var mightNull已取票 = _movieContext.TOrders.Include(o => o.FSession).ThenInclude(s => s.FTheater).
            //        Include(o => o.FSession).ThenInclude(s => s.FMovie).
            //        FirstOrDefault(o => o.FMemberId == members.FMemberId & o.FOrderId == item.FOrderId);
            //    if (mightNull已取票 != null)
            //        this會員的已取票.Add(mightNull已取票);
            //}

            //var orderStatus_已取消 = _movieContext.TOrderStatuses.Where(od => od.FOrderStatus == "已取消").Select(od => od.FOrderStatusId).ToList();
            //List<TOrderStatusLog> order_已取消 = new List<TOrderStatusLog>();
            //foreach (var item in orderStatus_已取消)
            //    order_已取票.Add(_movieContext.TOrderStatusLogs.FirstOrDefault(osl => osl.FOrderStatusId == item));
            //List<TOrder> this會員的已取消 = new List<TOrder>();
            //foreach (var item in order_已取消)
            //{
            //    var mightNull已取消 = _movieContext.TOrders.Include(o => o.FSession).ThenInclude(s => s.FTheater).
            //        Include(o => o.FSession).ThenInclude(s => s.FMovie).
            //        FirstOrDefault(o => o.FMemberId == members.FMemberId & o.FOrderId == item.FOrderId);
            //    if (mightNull已取消 != null)
            //        this會員的已取票.Add(mightNull已取消);
            //}

            var viewModel = new COrderAndMemberViewModel
            {
                Member = members,
                //Order_未取票 = this會員的未取票,
                //Order_已取票 = this會員的已取票,
                //Order_已取消 = this會員的已取消,
                CouponList = queryCoupon(id),
                MemberActionNow=queryFavorite(id),
            };
            sessionCheck();
            return View(viewModel);
        }
        public IActionResult commentList(int? id)
        {
            var members = _movieContext.TMembers.FirstOrDefault(c => c.FMemberId == id);
            var shortCmt =_movieContext.TShortCmts.Include(c=>c.FMovie)                
                .Where(c=>c.FMemberId == id).OrderByDescending(c=>c.FCreatedTime)
                .ToList();
            var viewModel = new CCommentAndMemberViewModel
            {
                Member = members,
                ShortCmt = shortCmt,
                CouponList = queryCoupon(id),
                MemberActionNow=queryFavorite(id),
            };
            sessionCheck();
            return View(viewModel);
        }

        public List<TCouponList> queryCoupon(int? id)
        {
            var couponList = _movieContext.TCouponLists.Include(c => c.FCouponType).Where(c => c.FMemberId == id & c.FCouponType.FCouponDueDate.Month == DateTime.Now.Month & c.FIsUsed == false)
                .OrderByDescending(c => c.FCouponType.FCouponDueDate).ToList();
            return couponList;
        }
        public List<TMemberAction> queryFavorite(int? id)
        {
            var memberActionNow = _movieContext.TMemberActions.Include(c => c.FMovie)
             .Where(c => c.FMemberId == id & c.FMovie.FScheduleStart < DateTime.Now & c.FMovie.FScheduleEnd > DateTime.Now & c.FActionTypeId == 1).ToList();
            return memberActionNow;
        }
        public int queryTotalPrice(int? id)
        {
            var totalPrice=_movieContext.TOrders.
                Include(o=>o.TOrderStatusLogs).
                ThenInclude(o=>o.FOrderStatus).
                Where(o=>o.FMemberId==id & o.FOrderDate.Year==DateTime.Now.Year).Select(o=>o.FOrderId).ToList();
            var totalPrice2 = _movieContext.TOrderStatusLogs.Where(o => totalPrice.Contains(o.FOrderId)& o.FOrderStatus.FOrderStatus=="已取消").Select(o=>o.FOrderId).ToList();
            var totalPriceSum = _movieContext.TOrders.Where(o => o.FMemberId == id & o.FOrderDate.Year == DateTime.Now.Year).ToList();
            var cancel=_movieContext.TOrders.Where(o => o.FMemberId == id & o.FOrderDate.Year == DateTime.Now.Year & totalPrice2.Contains(o.FOrderId)).ToList();


            int price = 0;
            int cancelprice = 0;
            foreach (var item in totalPriceSum)
            {
                price += (int)item.FTotalPrice;
            }
            foreach (var item2 in cancel)
            {
                cancelprice += (int)item2.FTotalPrice;
            }
            price =price-cancelprice;
            return price;
        }
    }
}
