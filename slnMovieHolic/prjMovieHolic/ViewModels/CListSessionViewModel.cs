using prjMovieHolic.Models;

namespace prjMovieHolic.ViewModels
{
    public class CListSessionViewModel
    {
        //ListSession
        public TMovie tMovie { get; set; }
        public int MovieID { get; set; }
        public string MovieName { get; set; }
        public string[] tTypeListNames { get; set; }
        public string TypeListNames { get; set; }
        public string[] tActorListNames { get; set; }
        public string ActorListNames { get; set; }
        public string[] tDirectorListNames { get; set; }
        public string DirectorListNames { get; set; }
        public string[] weekDays { get; set; }

    }
}
