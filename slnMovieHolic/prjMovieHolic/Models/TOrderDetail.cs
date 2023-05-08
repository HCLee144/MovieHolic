using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TOrderDetail
{
    public int FOrderDetailId { get; set; }

    public int FOrderId { get; set; }

    public int FSeatId { get; set; }

    public int FTicketClassId { get; set; }

    public virtual TOrder FOrder { get; set; } = null!;

    public virtual TSeat FSeat { get; set; } = null!;

    public virtual TTicketClass FTicketClass { get; set; } = null!;
}
