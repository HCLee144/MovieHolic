using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TmActorList
{
    public int FId { get; set; }

    public int FMovieId { get; set; }

    public int FActorId { get; set; }

    public string? FCharactorNameCht { get; set; }

    public virtual TmActor FActor { get; set; } = null!;

    public virtual TmMovie FMovie { get; set; } = null!;
}
