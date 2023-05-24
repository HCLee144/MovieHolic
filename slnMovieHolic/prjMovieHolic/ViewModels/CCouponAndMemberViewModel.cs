using Microsoft.AspNetCore.Mvc;
using prjMovieHolic.Models;

namespace prjMovieHolic.ViewModels
{
	public class CCouponAndMemberViewModel
	{
		public TMember Member { get; set; }
		public List<TCouponList> CouponList { get; set;}
		public List<TCouponList> CouponListExpired { get; set; }
		public List<TCouponList> CouponListUsed { get; set; }
		public TMovie Movie { get; set; }
        public List<TMemberAction> MemberActionNow { get; set; }
    }

}
