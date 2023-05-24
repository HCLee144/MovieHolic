using prjMovieHolic.Models;

namespace prjMovieHolic.ViewModels
{
    public class CMovieAndMemberViewModel
    {
        public TMember Member { get; set; }

        public List<TMemberAction> MemberActionNow { get; set; }
        public List<TMemberAction> MemberActionFuture { get; set; }
        public List<TMemberAction> MemberActionExpired { get; set; }
		public List<TCouponList> CouponList { get; set; }

	}
}
