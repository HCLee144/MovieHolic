using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class CouponType
{
    public int CouponTypeId { get; set; }

    public string CouponTypeName { get; set; } = null!;

    public int CouponDiscount { get; set; }

    public DateTime CouponStartDate { get; set; }

    public DateTime CouponDueDate { get; set; }

    public virtual ICollection<CouponList> CouponLists { get; set; } = new List<CouponList>();
}
