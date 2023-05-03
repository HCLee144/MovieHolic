using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TmImageList
{
    public int FId { get; set; }

    public int FMovieId { get; set; }

    public int FImageId { get; set; }

    public virtual TmImage FImage { get; set; } = null!;

    public virtual TmMovie FMovie { get; set; } = null!;
}
