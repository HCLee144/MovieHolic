using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class OrderStatusLog
{
    public int OrderStatusLogId { get; set; }

    public int OrderStatusId { get; set; }

    public int OrderId { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual OrderStatus OrderStatus { get; set; } = null!;
}
