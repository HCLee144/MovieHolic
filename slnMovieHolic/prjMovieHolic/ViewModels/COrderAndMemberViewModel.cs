using prjMovieHolic.Models;

namespace prjMovieHolic.ViewModels
{
    public class COrderAndMemberViewModel
    {
        public TMember Member { get; set; }
        public List<TOrder> Order { get; set; }
        public List<TOrderDetail> OrderDetail { get; set; }
    }
}
