using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int SessionId { get; set; }

    public int MemberId { get; set; }

    public int PayTypeId { get; set; }

    public int? CreditCardTypeId { get; set; }

    public int? CouponId { get; set; }

    public DateTime OrderDate { get; set; }

    public int? InvoiceNumber { get; set; }

    public int? TotalPrice { get; set; }

    public virtual CreditCardType? CreditCardType { get; set; }

    public virtual Member Member { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<OrderStatusLog> OrderStatusLogs { get; set; } = new List<OrderStatusLog>();

    public virtual PayType PayType { get; set; } = null!;

    public virtual Session Session { get; set; } = null!;
}
