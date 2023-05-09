using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjMovieHolic.Models;
using prjMovieHolic.ViewModels;
using System.Drawing.Drawing2D;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace prjMovieHolic.Controllers
{
    public class memberFrontController : Controller
    {
        private readonly MovieContext _movieContext;
        public memberFrontController(MovieContext context)
        {
            _movieContext= context;
        }
        
        //會員登入
        public IActionResult memberLogin(CMemberViewModel vm)
        { 
            TMember user=_movieContext.TMembers.FirstOrDefault(t=>t.FPhone.Equals(vm.txtAccount)&&t.FPassword.Equals(vm.txtPassword));
            if(user != null && user.FPassword.Equals(vm.txtPassword))
            {
                //string json=JsonSerializer.Serialize(user.);
                HttpContext.Session.SetInt32(CDictionary.SK_LOGIN_USER,user.FMemberId);
                ViewBag.Login = true;
                ViewBag.userId = user.FMemberId;
                return RedirectToAction("Index","Home");
            }
            return View();
        }
        public IActionResult memberLogout()
        {//todo 登出
            HttpContext.Session.Remove(CDictionary.SK_LOGIN_USER);
            return RedirectToAction("Index","Home");
        }
        //會員基本資料
        public IActionResult memberList(int? id)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_USER))
            {
            TMember memberData = _movieContext.TMembers.Include(t => t.FMembership)
                .Include(t => t.FGender)
                .FirstOrDefault(t => t.FMemberId == id);
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
            return View(memberData);
        }
        [HttpPost]
        public IActionResult passwordEdit(CMemberViewModel vm)
        { //todo 密碼驗證
            var memberData = _movieContext.TMembers.FirstOrDefault(t=>t.FMemberId==vm.FMemberId);
            if (memberData != null)
            {
                var password = _movieContext.TMembers.FirstOrDefault(t => t.FPassword == vm.txtPreviousFPassword);
                bool passwordDoubleCheck = vm.txtNewFPassword.Equals(vm.txtNewFPasswordCheck);
                if (password != null && passwordDoubleCheck==true)
                {
                    memberData.FPassword = vm.txtNewFPasswordCheck;
                    _movieContext.SaveChanges();
                    return RedirectToAction("memberList", new { id = vm.FMemberId });
                }
                return View();
            }
            return View();
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
            bool passwordFormat= !string.IsNullOrEmpty(newPassword) && Regex.IsMatch(newPassword, @"(?=.{10,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])");
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
