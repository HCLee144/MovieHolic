using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TMembershipChangeLog
{
    public int FMembershipChangeLogId { get; set; }

    public int FMemberId { get; set; }

    public int FMembershipId { get; set; }

    public DateTime FChangeDate { get; set; }
}
