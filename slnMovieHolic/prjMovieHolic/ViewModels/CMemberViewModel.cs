using prjMovieHolic.Models;
using System.ComponentModel.DataAnnotations;

namespace prjMovieHolic.ViewModels
{
    public class CMemberViewModel
    {
        public int FMemberId { get; set; }

        public string txtPreviousFPassword { get; set; }

        public string txtNewFPassword { get; set; }

        public string txtNewFPasswordCheck { get; set; }

        public string txtAccount { get; set; }
        public string txtPassword { get; set; }
    }
}
