using prjMovieHolic.Models;
using System.ComponentModel.DataAnnotations;

namespace prjMovieHolic.ViewModels
{
    public class CMemberViewModel
    {
        //修改密碼
        public int FMemberId { get; set; }

        public string txtPreviousFPassword { get; set; }

        public string txtNewFPassword { get; set; }

        public string txtNewFPasswordCheck { get; set; }
        //登入
        public string txtAccount { get; set; }
        public string txtPassword { get; set; }
        //忘記密碼
        public string txtForgetPasswordEmail { get; set; }
    }
}
