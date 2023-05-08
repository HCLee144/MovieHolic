using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TCouponType
{
    public int FCouponTypeId { get; set; }

    public string FCouponTypeName { get; set; } = null!;

    public int FCouponDiscount { get; set; }

    public DateTime FCouponStartDate { get; set; }

    public DateTime FCouponDueDate { get; set; }

    public virtual ICollection<TCouponList> TCouponLists { get; set; } = new List<TCouponList>();
}
