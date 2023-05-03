using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TmDirector
{
    public int FId { get; set; }

    public string? FNameCht { get; set; }

    public string? FNameEng { get; set; }

    public byte[]? FImagePath { get; set; }

    public virtual ICollection<TmDirectorList> TmDirectorLists { get; set; } = new List<TmDirectorList>();
}
