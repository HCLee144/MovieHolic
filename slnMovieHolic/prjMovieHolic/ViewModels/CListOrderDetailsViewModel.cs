namespace prjMovieHolic.ViewModels
{
    public class CListOrderDetailsViewModel
    {
        public string selectedMovieName { get; set; }
        public int selectedMovieID { get; set; }
        public string selecteDate { get; set; }
        public string theaterName { get; set; }
        public string tickets { get; set; }
        public List<string> seats { get; set; }
        public List<string> set { get; set; }
        public string discount { get; set; }
        public string totalPrice{get;set;}
    }
}
