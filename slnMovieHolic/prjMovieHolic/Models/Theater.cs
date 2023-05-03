using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class Theater
{
    public int TheaterId { get; set; }

    public string Theater1 { get; set; } = null!;

    public int TheaterClassId { get; set; }

    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();

    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();

    public virtual TheaterClass TheaterClass { get; set; } = null!;
}
