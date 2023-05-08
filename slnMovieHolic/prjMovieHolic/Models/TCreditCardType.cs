using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TCreditCardType
{
    public int FCreditCardTypeId { get; set; }

    public string? FCreditCardType { get; set; }

    public double? FPriceRate { get; set; }

    public virtual ICollection<TOrder> TOrders { get; set; } = new List<TOrder>();
}
