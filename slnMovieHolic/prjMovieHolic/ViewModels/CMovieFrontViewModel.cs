using prjMovieHolic.Models;

namespace prjMovieHolic.ViewModels
{
    public class CMovieFrontViewModel
    {
        public TMovie tMovie { get; set; }
        public string[] tTypeListNames { get; set; }
        public string TypeListNames { get; set; }
        public string[] tActorListNames { get; set; }
        public string ActorListNames { get; set; }
        public string[] tDirectorListNames { get; set; }
        public string DirectorListNames { get; set; }
        public List<TMovie>? NowShowingMovies { get; set; }
        public List<TMovie>? UpcomingMovies { get; set; }

    }
}
