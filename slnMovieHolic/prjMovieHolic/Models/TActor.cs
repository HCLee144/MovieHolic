using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TActor
{
    public int FId { get; set; }

    public string? FActorNameCht { get; set; }

    public string? FActorNameEng { get; set; }

    public string? FActorImagePath { get; set; }

    public virtual ICollection<TActorList> TActorLists { get; set; } = new List<TActorList>();
}
