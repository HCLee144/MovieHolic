using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TmDirectorList
{
    public int FId { get; set; }

    public int FMovieId { get; set; }

    public int FDirectorId { get; set; }

    public virtual TmDirector FDirector { get; set; } = null!;

    public virtual TmMovie FMovie { get; set; } = null!;
}
