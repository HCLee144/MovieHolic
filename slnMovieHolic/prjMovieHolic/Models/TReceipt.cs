using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TReceipt
{
    public int FReceiptId { get; set; }

    public int FMemberId { get; set; }

    public DateTime FReceiptDate { get; set; }

    public int? FOrderId { get; set; }

    public virtual TMember FMember { get; set; } = null!;

    public virtual ICollection<TReceiptDetail> TReceiptDetails { get; set; } = new List<TReceiptDetail>();
}
