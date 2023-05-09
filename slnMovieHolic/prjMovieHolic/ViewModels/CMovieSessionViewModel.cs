using prjMovieHolic.Models;

namespace prjMovieHolic.ViewModels
{
    public class CSessionBackViewModel
    {
        public string? queryDate { get; set; }

        public string? selectedSessionTheaterName { get; set; }

        public string? selectedSessionStartString { get; set; }

        public int? sessionID { get; set; }
        public string? TheaterName { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
        public bool hasOrder { get; set; }
        public int MovieId { get; set; }

        public string? MovieName { get; set; }

        public string? MoviePosterPath { get; set; }




    }
}


