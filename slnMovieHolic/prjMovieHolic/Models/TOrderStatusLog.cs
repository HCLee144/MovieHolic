using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TOrderStatusLog
{
    public int FOrderStatusLogId { get; set; }

    public int FOrderStatusId { get; set; }

    public int FOrderId { get; set; }

    public virtual TOrder FOrder { get; set; } = null!;

    public virtual TOrderStatus FOrderStatus { get; set; } = null!;
}
