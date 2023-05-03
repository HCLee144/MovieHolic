using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class Seat
{
    public int SeatId { get; set; }

    public int TheaterId { get; set; }

    public byte SeatRow { get; set; }

    public byte SeatNum { get; set; }

    public int SeatStatusId { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual SeatStatus SeatStatus { get; set; } = null!;

    public virtual Theater Theater { get; set; } = null!;
}
