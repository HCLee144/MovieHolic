using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TPayType
{
    public int FPayTypeId { get; set; }

    public string? FPayType { get; set; }

    public virtual ICollection<TOrder> TOrders { get; set; } = new List<TOrder>();
}
