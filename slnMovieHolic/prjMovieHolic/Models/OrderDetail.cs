using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int OrderId { get; set; }

    public int SeatId { get; set; }

    public int TicketClassId { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Seat Seat { get; set; } = null!;

    public virtual TicketClass TicketClass { get; set; } = null!;
}
