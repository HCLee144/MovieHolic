using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TmImage
{
    public int FId { get; set; }

    public string? FImagePath { get; set; }

    public virtual ICollection<TmImageList> TmImageLists { get; set; } = new List<TmImageList>();
}
