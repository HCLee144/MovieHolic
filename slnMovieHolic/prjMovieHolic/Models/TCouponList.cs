using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TCouponList
{
    public int FCouponId { get; set; }

    public int FCouponTypeId { get; set; }

    public int FMemberId { get; set; }

    public DateTime FReceiveDate { get; set; }

    public int? FOrderId { get; set; }

    public bool FIsUsed { get; set; }

    public virtual TCouponType FCouponType { get; set; } = null!;

    public virtual TMember FMember { get; set; } = null!;
}
