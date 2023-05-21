using prjMovieHolic.Models;

namespace prjMovieHolic.ViewModels
{
    public class CListProductViewModel
    {
        public List<CTProductWrap> drinkCategory { get; set; }
        public List<CTProductWrap> popcornCategory { get; set; }
        public List<CTProductWrap> dessertCategory { get; set; }

        public int selectedSessionID { get; set; }
        public string selectedMovieName { get; set; }
        public string selectedSessionDate { get; set; }
        public string selectedSessionTime { get; set; }
        public int selectedTheaterID { get; set; }
        public string selectedTheaterName { get; set; }
        public string ticketCounts { get; set; }
        public List<string> selectedSeats { get; set; }

    }

    public class CProductInfo
    {
        public int productID { get; set; }
        public string productCount { get; set; }
    }

    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // 其他屬性
    }
}
