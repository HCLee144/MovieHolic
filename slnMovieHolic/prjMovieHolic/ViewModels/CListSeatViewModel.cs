namespace prjMovieHolic.ViewModels
{
    public class CListSeatViewModel
    {
        public int sessionID_seat { get; set; }
        public int normalCount_seat { get; set; }
        public int studentCount_seat { get; set; }
        public int soldierCount_seat { get; set; }
        public int[] seatStatus { get; set; }
    }
}
