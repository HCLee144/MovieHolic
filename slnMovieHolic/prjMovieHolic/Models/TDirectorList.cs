using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TDirectorList
{
    public int FId { get; set; }

    public int FMovieId { get; set; }

    public int FDirectorId { get; set; }

    public virtual TDirector FDirector { get; set; } = null!;

    public virtual TMovie FMovie { get; set; } = null!;
}
