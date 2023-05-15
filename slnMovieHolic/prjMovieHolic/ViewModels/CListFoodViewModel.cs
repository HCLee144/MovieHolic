using prjMovieHolic.Models;

namespace prjMovieHolic.ViewModels
{
    public class CListFoodViewModel
    {
        public List<CTProductWrap> drinkCategory { get; set; }
        public List<CTProductWrap> popcornCategory { get; set; }
        public List<CTProductWrap> dessertCategory { get; set; }

        public int selectedSessionID { get; set; }
        public string selectedMovieName { get; set; }
        public string selectedSessionDate { get; set; }
        public string selectedSessionTime { get; set; }
        public string selectedTheater { get; set; }
        public string ticketCounts { get; set; }
        public string selectedSeats { get; set; }

    }
}
