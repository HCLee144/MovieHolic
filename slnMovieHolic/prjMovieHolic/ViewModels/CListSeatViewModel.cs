namespace prjMovieHolic.ViewModels
{
    public class CListSeatViewModel
    {
        public int sessionID_seat { get; set; }
        public int normalCount_seat { get; set; }
        public int studentCount_seat { get; set; }
        public int soldierCount_seat { get; set; }
        public int[] seatStatus { get; set; }

        //呈現在畫面上
        public string movieName { get; set; }
        public string theaterID { get; set; }
        public string theaterName { get; set; }
        public string sessionDate { get; set; }
        public string sessionTime { get; set; }
        public string totalTickets { get; set; }
    }
}
