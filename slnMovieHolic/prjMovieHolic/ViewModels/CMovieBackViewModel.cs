using Microsoft.Extensions.FileProviders;
using prjMovieHolic.Models;

namespace prjMovieHolic.ViewModels
{
    public class CMovieBackViewModel
    {
        public int FId { get; set; }

        public int? FSeriesId { get; set; }

        public int? FRatingId { get; set; }

        public string FNameCht { get; set; } = null!;

        public string FNameEng { get; set; } = null!;

        public DateTime FScheduleStart { get; set; }

        public DateTime FScheduleEnd { get; set; }

        public int? FShowLength { get; set; }

        public string? FInteroduce { get; set; }

        public string? FTrailerLink { get; set; }

        public string? FPosterPath { get; set; }

        public string? FImagePath { get; set; }

        public int? FPrice { get; set; }

        public IFormFile? image { get; set; }

    }
}
