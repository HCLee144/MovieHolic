using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TMovieCmt
{
    public int FCmtid { get; set; }

    public int FMovieId { get; set; }

    public int FMemberId { get; set; }

    public string? FTitle { get; set; }

    public int FRate { get; set; }

    public string? FText { get; set; }

    public DateTime FCreatedTime { get; set; }

    public DateTime? FEditedTime { get; set; }

    public bool? FVisible { get; set; }

    public virtual Member FMember { get; set; } = null!;

    public virtual TmMovie FMovie { get; set; } = null!;

    public virtual ICollection<TCmtImage> TCmtImages { get; set; } = new List<TCmtImage>();
}
