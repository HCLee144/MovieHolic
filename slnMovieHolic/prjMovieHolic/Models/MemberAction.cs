using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class MemberAction
{
    public int MemberActionId { get; set; }

    public int MemberId { get; set; }

    public int MovieId { get; set; }

    public byte ActionTypeId { get; set; }

    public DateTime TimeStamp { get; set; }

    public virtual ActionType ActionType { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;

    public virtual TmMovie Movie { get; set; } = null!;
}
