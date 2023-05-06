using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TSession
{
    public int FSessionId { get; set; }

    public int FTheaterId { get; set; }

    public int FMovieId { get; set; }

    public DateTime FStartTime { get; set; }

    public DateTime FEndTime { get; set; }

    public virtual TMovie FMovie { get; set; } = null!;

    public virtual TTheater FTheater { get; set; } = null!;

    public virtual ICollection<TOrder> TOrders { get; set; } = new List<TOrder>();
}
