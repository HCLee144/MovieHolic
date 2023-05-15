namespace prjMovieHolic.ViewModels
{
    public class CListTicketViewModel
    {
        //ListTicketClass
        public int selectedSessionID { get; set; }
        public string selectedSessionDateTime { get; set; }
        public string selectedTheater { get; set; }
        public string selectedMovieName { get; set; }

        public int oneNormalPrice { get; set; }
        public int oneStudentPrice { get; set; }
        public int oneSoldierPrice { get; set; }
    }
}
