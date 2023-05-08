using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TSeat
{
    public int FSeatId { get; set; }

    public int FTheaterId { get; set; }

    public byte FSeatRow { get; set; }

    public byte FSeatNum { get; set; }

    public int FSeatStatusId { get; set; }

    public virtual TSeatStatus FSeatStatus { get; set; } = null!;

    public virtual TTheater FTheater { get; set; } = null!;

    public virtual ICollection<TOrderDetail> TOrderDetails { get; set; } = new List<TOrderDetail>();
}
