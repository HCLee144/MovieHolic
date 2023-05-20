using prjMovieHolic.Models;

namespace prjMovieHolic.ViewModels
{
    public class COrderAndMemberViewModel
    {
        public TMember Member { get; set; }
        public List<TOrder> Order_未取票 { get; set; }
        public List<TOrder> Order_已取票 { get; set; }
        public List<TOrder> Order_已取消 { get; set; }
        public List<TOrderDetail> OrderDetail { get; set; }
    }
}
