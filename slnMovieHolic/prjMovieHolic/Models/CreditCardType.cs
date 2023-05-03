using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class CreditCardType
{
    public int CreditCardTypeId { get; set; }

    public string? CreditCardType1 { get; set; }

    public double? PriceRate { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
