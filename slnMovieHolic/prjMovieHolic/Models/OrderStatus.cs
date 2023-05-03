using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class OrderStatus
{
    public int OrderStatusId { get; set; }

    public string OrderStatus1 { get; set; } = null!;

    public DateTime ChangedTime { get; set; }

    public virtual ICollection<OrderStatusLog> OrderStatusLogs { get; set; } = new List<OrderStatusLog>();
}
