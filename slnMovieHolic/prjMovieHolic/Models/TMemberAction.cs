using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TMemberAction
{
    public int FMemberActionId { get; set; }

    public int FMemberId { get; set; }

    public int FMovieId { get; set; }

    public byte FActionTypeId { get; set; }

    public DateTime FTimeStamp { get; set; }

    public virtual TActionType FActionType { get; set; } = null!;

    public virtual TMember FMember { get; set; } = null!;

    public virtual TMovie FMovie { get; set; } = null!;
}
