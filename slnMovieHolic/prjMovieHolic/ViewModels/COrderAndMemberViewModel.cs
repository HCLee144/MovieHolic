using prjMovieHolic.Models;

namespace prjMovieHolic.ViewModels
{
    public class COrderAndMemberViewModel
    {
        public TMember Member { get; set; }
        public List<TOrder> OrderPicked { get; set; }
        public List<TOrder> OrderUnpicked { get; set; }
        public List<TOrder> OrderCancel {  get; set; }
        public List<TOrderDetail> OrderDetail { get; set; }
    }
}
