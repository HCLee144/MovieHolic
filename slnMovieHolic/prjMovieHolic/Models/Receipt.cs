using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class Receipt
{
    public int ReceiptId { get; set; }

    public int MemberId { get; set; }

    public DateTime ReceiptDate { get; set; }

    public int? OrderId { get; set; }

    public virtual Member Member { get; set; } = null!;

    public virtual ICollection<ReceiptDetail> ReceiptDetails { get; set; } = new List<ReceiptDetail>();
}
