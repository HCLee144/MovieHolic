using prjMovieHolic.Models;

namespace prjMovieHolic.ViewModels
{
    public class CMovieFrontViewModel
    {
        public TMovie tMovie {  get; set; }
        public TType tType { get; set; }
        public string[] tTypeListNames { get; set; }
        public string TypeListNames { get; set; }
        public string[] tActorListNames { get; set; }
        public string ActorListNames { get; set; }
        public string[] tDirectorListNames { get; set; }
        public string DirectorListNames { get; set; }

        public List<TMovie> tMovies { get; set; }
        public List<TType> tTypes { get; set; }
        public List<TMovie>? NowShowingMovies { get; set; }
        public List<TMovie>? UpcomingMovies { get; set; }

        //新增首頁電影收藏用
        public TMember tmember { get; set; }
        public List<TMemberAction> isFavoriteNow { get; set; }
        public List<TMemberAction> isFavoriteComing { get; set; }
        public List<TMemberAction> isFavoriteAll { get; set; }

        //快速訂票用--Ting
        public List<TSession> getTickets { get; set; }

        //輪播牆-Beautiful Ting
        public string[] movieImagePaths { get; set; }
    }
}
