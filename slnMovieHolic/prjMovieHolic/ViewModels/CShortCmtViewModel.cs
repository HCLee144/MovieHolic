using prjMovieHolic.Models;

namespace prjMovieHolic.ViewModels
{
    public class CShortCmtViewModel
    {
        public int CmtID { get; set; }
        public int memberID { get; set; }
        public TMember member { get; set; }
        public int movieID { get; set; }
        public TMovie movie { get; set; }
        public string title { get; set; }
        public int rate { get; set; }

    }
}
