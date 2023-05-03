using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class CouponList
{
    public int CouponId { get; set; }

    public int CouponTypeId { get; set; }

    public int MemberId { get; set; }

    public DateTime ReceiveDate { get; set; }

    public int? OrderId { get; set; }

    public bool IsUsed { get; set; }

    public virtual CouponType CouponType { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;
}
