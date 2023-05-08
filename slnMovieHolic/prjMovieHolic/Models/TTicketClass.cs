using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TTicketClass
{
    public int FTicketClassId { get; set; }

    public string FTicketClass { get; set; } = null!;

    public decimal FPriceRate { get; set; }

    public virtual ICollection<TOrderDetail> TOrderDetails { get; set; } = new List<TOrderDetail>();
}
