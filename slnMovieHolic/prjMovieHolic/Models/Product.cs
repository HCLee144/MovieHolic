using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public int ProductPrice { get; set; }

    public byte[]? Image { get; set; }

    public string? ImagePath { get; set; }

    public string Introduce { get; set; } = null!;

    public int CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<ReceiptDetail> ReceiptDetails { get; set; } = new List<ReceiptDetail>();
}
