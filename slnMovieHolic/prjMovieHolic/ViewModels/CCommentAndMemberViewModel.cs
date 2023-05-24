using prjMovieHolic.Models;

namespace prjMovieHolic.ViewModels
{
    public class CCommentAndMemberViewModel
    {
        public List<TArticle> Article { get; set; }
        public TMember Member { get; set; }
        public List<TShortCmt>? ShortCmt { get; set; }
		public List<TCouponList> CouponList { get; set; }
        public List<TMemberAction> MemberActionNow { get; set; }



    }
}
