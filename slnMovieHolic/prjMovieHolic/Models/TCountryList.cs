using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TCountryList
{
    public int FId { get; set; }

    public int FMovieId { get; set; }

    public int FCountryId { get; set; }

    public virtual TCountry FCountry { get; set; } = null!;

    public virtual TMovie FMovie { get; set; } = null!;
}
