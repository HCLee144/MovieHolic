using Microsoft.AspNetCore.Http.HttpResults;

namespace prjMovieHolic.ViewModels
{
    public class CCmtViewModel
    {

        public string member_name { get; set; }
        public int rating { get; set; }
        public string title { get; set; }
        public DateTime created_time { get; set; }
    }
}
