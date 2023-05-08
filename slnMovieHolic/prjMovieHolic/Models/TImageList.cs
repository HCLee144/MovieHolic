using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TImageList
{
    public int FId { get; set; }

    public int FMovieId { get; set; }

    public int FImageId { get; set; }

    public virtual TImage FImage { get; set; } = null!;

    public virtual TMovie FMovie { get; set; } = null!;
}
