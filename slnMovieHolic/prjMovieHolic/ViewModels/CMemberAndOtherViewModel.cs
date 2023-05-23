using prjMovieHolic.Models;

namespace prjMovieHolic.ViewModels
{
    public class CMemberAndOtherViewModel
    {
        public TMember Member { get; set; }
        public List<TCouponList> CouponList { get; set; }
        public List<TMemberAction> MemberActionNow { get; set; }
        public int TotalPrice { get; set; }
    }
}
