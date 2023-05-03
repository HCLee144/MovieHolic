using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class PayType
{
    public int PayTypeId { get; set; }

    public string? PayType1 { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
