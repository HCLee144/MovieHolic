using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TmSeries
{
    public int FId { get; set; }

    public string FNameCht { get; set; } = null!;

    public virtual ICollection<TmMovie> TmMovies { get; set; } = new List<TmMovie>();
}
