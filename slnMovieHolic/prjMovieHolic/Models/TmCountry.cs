using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TmCountry
{
    public int FId { get; set; }

    public string FNameCht { get; set; } = null!;

    public string? FNameEng { get; set; }

    public string? FImagePath { get; set; }

    public virtual ICollection<TmCountryList> TmCountryLists { get; set; } = new List<TmCountryList>();
}
