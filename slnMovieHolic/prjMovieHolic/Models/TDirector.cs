using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TDirector
{
    public int FId { get; set; }

    public string? FNameCht { get; set; }

    public string? FNameEng { get; set; }

    public byte[]? FImagePath { get; set; }

    public virtual ICollection<TDirectorList> TDirectorLists { get; set; } = new List<TDirectorList>();
}
