using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TImage
{
    public int FId { get; set; }

    public string? FImagePath { get; set; }

    public virtual ICollection<TImageList> TImageLists { get; set; } = new List<TImageList>();
}
