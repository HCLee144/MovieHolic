using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjMovieHolic.Models;
using prjMovieHolic.ViewModels;
using System.Drawing.Drawing2D;

namespace prjMovieHolic.Controllers
{
    public class memberFrontController : Controller
    {
        private readonly MovieContext _movieContext;
        public memberFrontController(MovieContext context)
        {
            _movieContext= context;
        }

        public IActionResult memberList(int? id)
        {

            TMember memberData = _movieContext.TMembers.Include(t => t.FMembership)
                .Include(t => t.FGender)
                .FirstOrDefault(t => t.FMemberId == id);
            return View(memberData);

        }
        public IActionResult memberLogin() 
        {
            return View();
        }
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
                //var password =_movieContext.TMembers.FirstOrDefault(t => t.FPassword == vm.txtPreviousFPassword);
                //if (password != null)
                //{
                    memberData.FPassword = vm.txtNewFPassword;
                    _movieContext.SaveChanges();
                //}
            }
            return RedirectToAction("memberList", new { id = vm.FMemberId });
        }
        public IActionResult passwordCheck(string previousPassword)
        {
            var password = _movieContext.TMembers.Any(t => t.FPassword == previousPassword);
            return Content(password.ToString());
        }
    }
}
