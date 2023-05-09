using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TRating
{
    public int FId { get; set; }

    public string? FNameCht { get; set; }

    public string? FImagePath { get; set; }

    public virtual ICollection<TMovie> TMovies { get; set; } = new List<TMovie>();
}
