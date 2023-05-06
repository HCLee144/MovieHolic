using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TOrder
{
    public int FOrderId { get; set; }

    public int FSessionId { get; set; }

    public int FMemberId { get; set; }

    public int FPayTypeId { get; set; }

    public int? FCreditCardTypeId { get; set; }

    public int? FCouponId { get; set; }

    public DateTime FOrderDate { get; set; }

    public int? FInvoiceNumber { get; set; }

    public int? FTotalPrice { get; set; }

    public virtual TCreditCardType? FCreditCardType { get; set; }

    public virtual TMember FMember { get; set; } = null!;

    public virtual TPayType FPayType { get; set; } = null!;

    public virtual TSession FSession { get; set; } = null!;

    public virtual ICollection<TOrderDetail> TOrderDetails { get; set; } = new List<TOrderDetail>();

    public virtual ICollection<TOrderStatusLog> TOrderStatusLogs { get; set; } = new List<TOrderStatusLog>();
}
