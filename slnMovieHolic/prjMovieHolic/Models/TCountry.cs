using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TCountry
{
    public int FId { get; set; }

    public string FNameCht { get; set; } = null!;

    public string? FNameEng { get; set; }

    public string? FImagePath { get; set; }

    public virtual ICollection<TCountryList> TCountryLists { get; set; } = new List<TCountryList>();
}
