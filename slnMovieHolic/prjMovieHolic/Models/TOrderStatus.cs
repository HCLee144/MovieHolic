using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TOrderStatus
{
    public int FOrderStatusId { get; set; }

    public string FOrderStatus { get; set; } = null!;

    public DateTime FChangedTime { get; set; }

    public virtual ICollection<TOrderStatusLog> TOrderStatusLogs { get; set; } = new List<TOrderStatusLog>();
}
