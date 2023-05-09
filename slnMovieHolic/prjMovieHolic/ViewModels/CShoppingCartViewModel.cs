using prjMovieHolic.Models;

namespace prjMovieHolic.ViewModels
{
    public class CShoppingCartViewModel
    {
        public TMovie tMovie { get; set; }
        public string[] tTypeListNames { get; set; }
        public string TypeListNames { get; set; }
        public TActorList tActorList { get; set; }
        public TDirectorList tDirectorList { get; set; }
        /*public TSession[] tSession
        {
            get
            {
                MovieContext movieContext = new MovieContext();
                return movieContext.TSessions.Where(s => s.FMovie == this.tMovie).ToArray();

            }
        }*/

        public string[] weekDays { get; set; }
    }
}
