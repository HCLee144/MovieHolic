using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class ActionType
{
    public byte ActionTypeId { get; set; }

    public string ActionTypeName { get; set; } = null!;

    public virtual ICollection<MemberAction> MemberActions { get; set; } = new List<MemberAction>();
}
