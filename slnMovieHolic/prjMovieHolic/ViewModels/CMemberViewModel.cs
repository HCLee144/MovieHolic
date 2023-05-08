//using Microsoft.Build.Framework;
using prjMovieHolic.Models;
using System.ComponentModel.DataAnnotations;

namespace prjMovieHolic.ViewModels
{
    public class CMemberViewModel
    {//todo 驗證?
        public int FMemberId { get; set; }
        [Required]
        public string txtPreviousFPassword { get; set; }
        [Required]
        public string txtNewFPassword { get; set; }
        [Required]
        [Compare("txtNewFPassword",ErrorMessage = "請確認您的新密碼是否正確")]
        public string txtNewFPasswordCheck { get; set; }

    }
}
