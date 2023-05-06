using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TActorList
{
    public int FId { get; set; }

    public int FMovieId { get; set; }

    public int FActorId { get; set; }

    public string? FCharactorNameCht { get; set; }

    public virtual TActor FActor { get; set; } = null!;

    public virtual TMovie FMovie { get; set; } = null!;
}
