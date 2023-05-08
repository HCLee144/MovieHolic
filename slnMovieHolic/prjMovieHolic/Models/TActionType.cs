using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TActionType
{
    public byte FActionTypeId { get; set; }

    public string FActionTypeName { get; set; } = null!;

    public virtual ICollection<TMemberAction> TMemberActions { get; set; } = new List<TMemberAction>();
}
