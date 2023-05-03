using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class Session
{
    public int SessionId { get; set; }

    public int TheaterId { get; set; }

    public int MovieId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public virtual TmMovie Movie { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Theater Theater { get; set; } = null!;
}
