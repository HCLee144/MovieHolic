using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class SeatStatus
{
    public int SeatStatusId { get; set; }

    public string SeatStatus1 { get; set; } = null!;

    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
}
