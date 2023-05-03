using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TicketClass
{
    public int TicketClassId { get; set; }

    public string TicketClass1 { get; set; } = null!;

    public decimal PriceRate { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
