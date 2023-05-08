using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TReceiptDetail
{
    public int FReceiptDetailId { get; set; }

    public int FReceiptId { get; set; }

    public int FProductId { get; set; }

    public int FQty { get; set; }

    public virtual TProduct FProduct { get; set; } = null!;

    public virtual TReceipt FReceipt { get; set; } = null!;
}
