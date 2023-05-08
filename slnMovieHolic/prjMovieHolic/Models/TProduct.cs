using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TProduct
{
    public int FProductId { get; set; }

    public string FProductName { get; set; } = null!;

    public int FProductPrice { get; set; }

    public byte[]? FImage { get; set; }

    public string? FImagePath { get; set; }

    public string FIntroduce { get; set; } = null!;

    public int FCategoryId { get; set; }

    public virtual TCategory FCategory { get; set; } = null!;

    public virtual ICollection<TReceiptDetail> TReceiptDetails { get; set; } = new List<TReceiptDetail>();
}
