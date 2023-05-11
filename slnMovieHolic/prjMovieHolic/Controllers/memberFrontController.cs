using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjMovieHolic.Models;
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
        {
            TMember user=_movieContext.TMembers.FirstOrDefault(t=>t.FPhone.Equals(vm.txtAccount));
            //bool verifyPassword = CPasswordHasher.VerifyPassword(vm.txtPassword, user.FPassword);
            if (user != null && user.FPassword.Equals(vm.txtPassword))
            {
                //string json=JsonSerializer.Serialize(user.);
                HttpContext.Session.SetInt32(CDictionary.SK_LOGIN_USER,user.FMemberId);
                ViewBag.Login = true;
                ViewBag.userId = user.FMemberId;
                return RedirectToAction("Index","Home");
            }
            return View();
        }
        //登出
        public IActionResult memberLogout()
        {
            HttpContext.Session.Remove(CDictionary.SK_LOGIN_USER);
            return RedirectToAction("Index","Home");
        }
         
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
                var message = new { text = "該電子郵件地址不存在。", type = "success" };
                return View("forgetPassword", message);
            }
            else
            { //todo 已重新設定會跳轉頁面到登入 
                CForgetPassword CforgetPassword = new CForgetPassword();
                CforgetPassword.getNewPasswordEmail(email);
                return RedirectToAction("memberLogin");
            }

        }
        //驗證是否成功
        public IActionResult IsForgetPassword(string txtForgetPasswordEmail)
        {
            CForgetPassword CforgetPassword = new CForgetPassword();
            bool isSent = _movieContext.TMembers.Where(o => o.FEmail == txtForgetPasswordEmail).ToList().Any();
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
                return RedirectToAction("memberList");
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
                memberData.FPhone = member.FPhone;
                memberData.FEmail = member.FEmail;
                _movieContext.SaveChanges();
            }
            return RedirectToAction("memberList", new { id = member.FMemberId });
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
            if (memberData != null)
            {
                var password = _movieContext.TMembers.FirstOrDefault(t => t.FPassword == vm.txtPreviousFPassword);
                bool passwordFormat = !string.IsNullOrEmpty(vm.txtNewFPassword) && Regex.IsMatch(vm.txtNewFPassword, @"(?=.{8,16})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])");
                bool passwordDoubleCheck = vm.txtNewFPassword.Equals(vm.txtNewFPasswordCheck);
                if (password != null && passwordFormat==true && passwordDoubleCheck==true)
                {
                    string newPassword=CPasswordHasher.HashPassword(vm.txtNewFPasswordCheck);
                    memberData.FPassword = newPassword;
                    _movieContext.SaveChanges();
                    return RedirectToAction("memberList", new { id = vm.FMemberId });
                }
                return View(memberData);
            }//todo 修改失敗時要有提示字
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

    }
}
