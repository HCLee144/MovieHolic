using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class MembershipChangeLog
{
    public int MembershipChangeLogId { get; set; }

    public int MemberId { get; set; }

    public int MembershipId { get; set; }

    public DateTime ChangeDate { get; set; }
}
