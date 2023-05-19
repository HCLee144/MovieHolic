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
        public IActionResult memberLogin(CMemberViewModel vm)
        {//todo 登入判斷包含id
            TMember user=_movieContext.TMembers.FirstOrDefault(t=>t.FPhone.Equals(vm.txtAccount));
            //bool verifyPassword = CPasswordHasher.VerifyPassword(vm.txtPassword, user.FPassword);
            if (user != null && user.FPassword.Equals(vm.txtPassword))
            {
                //string json=JsonSerializer.Serialize(user.);
                HttpContext.Session.SetInt32(CDictionary.SK_LOGIN_USER,user.FMemberId);
                ViewBag.Login = true;
                ViewBag.userId = user.FMemberId;
                string controller=HttpContext.Session.GetString(CDictionary.SK_CONTROLLER);
                string view = HttpContext.Session.GetString(CDictionary.SK_VIEW);
                ViewBag.Controller = controller;
                ViewBag.View = view;
                if (controller != null && view != null)
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
            return RedirectToAction("Index","Home");
        }
        //todo 尚未驗證完成 註冊會員
        public IActionResult memberSignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult memberSignUp(TMember member)
        {
            //if (ModelState.IsValid)
            //{
                var accountCheck = _movieContext.TMembers.Any(t => t.FPhone == member.FPhone);
                if (accountCheck == false)
                {
                    _movieContext.TMembers.Add(member);
                    _movieContext.SaveChanges();
                    return RedirectToAction("memberLogin");
                }
            //}
            return View();
                
        }
        //註冊：驗證帳號是否已存在
        public IActionResult accountCheck(string FPhone)
        {
            var accountCheck=_movieContext.TMembers.Any(t=>t.FPhone==FPhone);
            return Content(accountCheck.ToString());
        }
        //註冊：回傳是否註冊成功 
        //public IActionResult IsSignUp(string FPhone, string FPassword, string FPasswordCheck)
        //{
        //    //if (ModelState.IsValid)
        //    //{
        //        var accountCheck = _movieContext.TMembers.Any(t => t.FPhone == FPhone);
        //        bool passwordFormat = !string.IsNullOrEmpty(FPassword) && Regex.IsMatch(FPassword, @"(?=.{8,16})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])");
        //        bool passwordDoubleCheck = false;
        //        if (FPassword != null)
        //        {
        //            passwordDoubleCheck = FPassword.Equals(FPasswordCheck);
        //        }

        //        if (accountCheck != true && passwordFormat == true && passwordDoubleCheck == true)
        //        {
        //            return Json(new { success = true, message = "註冊成功。" });
        //        }
        //        else
        //        {
        //            return Json(new { success = false, message = "註冊失敗，請重新註冊。" });
        //        }
        //    //}
        //    //return View();
        //}



        //忘記密碼
        public IActionResult forgetPassword()
        {   //step1 驗證是否有此帳號
            //step2 寄信
            return View();
        }
        [HttpPost]
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

        //會員基本資料
        public IActionResult memberList(int? id)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_USER))
            {
            TMember memberData = _movieContext.TMembers.Include(t => t.FMembership)
                .Include(t => t.FGender)
                .FirstOrDefault(t => t.FMemberId == id);

                sessionCheck();

                return View(memberData);
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
                return View(memberData);
            }
            else
                return RedirectToAction("memberList",new {id=id});
        }
        [HttpPost]
        public IActionResult memberEdit(TMember member)
        {

            TMember memberData = _movieContext.TMembers.Include(t => t.FMembership)
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
            sessionCheck();
            return View(memberData);
        }
        [HttpPost]
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
                Movie=movie,
			};
            sessionCheck();
			return View(viewModel);
        }
        //收藏頁面顯示
        public IActionResult favoriteList(int? id)
        {
            var members=_movieContext.TMembers.FirstOrDefault(c=>c.FMemberId==id);
            var memberActionNow=_movieContext.TMemberActions.Include(c=>c.FMovie)
                .Where(c=>c.FMemberId==id & c.FMovie.FScheduleStart < DateTime.Now & c.FMovie.FScheduleEnd > DateTime.Now & c.FActionTypeId==1).ToList();
            var memberActionFuture= _movieContext.TMemberActions.Include(c => c.FMovie)
                .Where(c => c.FMemberId == id & c.FMovie.FScheduleStart > DateTime.Now & c.FActionTypeId==1) .ToList();
            var memberActionExpired = _movieContext.TMemberActions.Include(c => c.FMovie)
                .Where(c => c.FMemberId == id & c.FMovie.FScheduleEnd < DateTime.Now & c.FActionTypeId==1).ToList();
            var viewModel = new CMovieAndMemberViewModel
            {
                Member = members,
                MemberActionNow = memberActionNow,
                MemberActionFuture = memberActionFuture,
                MemberActionExpired = memberActionExpired
                
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
            return RedirectToAction("favoriteList", new { id = FMemberId });
        }
        public IActionResult orderList()
        {
            int id=(int)HttpContext.Session.GetInt32(CDictionary.SK_LOGIN_USER); 

            var members = _movieContext.TMembers.FirstOrDefault(c => c.FMemberId == id);
            var orderPicked=_movieContext.TOrders.Include(c=>c.FSession.FMovie).Include(c=>c.FSession).ThenInclude(c=>c.FTheater).Where(c=>c.FMemberId==id).OrderByDescending(o=>o.FOrderId).ToList();
            var viewModel = new COrderAndMemberViewModel
            {
                Member = members,
                OrderPicked = orderPicked,

            };
            sessionCheck();
            return View(viewModel);
        }

    }
}
