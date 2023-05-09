using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TShortCmt
{
    public int FCmtid { get; set; }

    public int FMovieId { get; set; }

    public int FMemberId { get; set; }

    public string? FTitle { get; set; }

    public int FRate { get; set; }

    public DateTime FCreatedTime { get; set; }

    public DateTime? FEditedTime { get; set; }

    public bool? FVisible { get; set; }

    public virtual TMember FMember { get; set; } = null!;

    public virtual TMovie FMovie { get; set; } = null!;
}
