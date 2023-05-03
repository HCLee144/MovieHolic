using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TmActor
{
    public int FId { get; set; }

    public string? FActorNameCht { get; set; }

    public string? FActorNameEng { get; set; }

    public string? FActorImagePath { get; set; }

    public virtual ICollection<TmActorList> TmActorLists { get; set; } = new List<TmActorList>();
}
