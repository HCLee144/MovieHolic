using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TSeries
{
    public int FId { get; set; }

    public string FNameCht { get; set; } = null!;

    public virtual ICollection<TMovie> TMovies { get; set; } = new List<TMovie>();
}
