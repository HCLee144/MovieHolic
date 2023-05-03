using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TmCountryList
{
    public int FId { get; set; }

    public int FMovieId { get; set; }

    public int FCountryId { get; set; }

    public virtual TmCountry FCountry { get; set; } = null!;

    public virtual TmMovie FMovie { get; set; } = null!;
}
