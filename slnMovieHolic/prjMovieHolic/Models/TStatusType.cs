using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TStatusType
{
    public int FStatusTypeId { get; set; }

    public string FStatusText { get; set; } = null!;

    public virtual ICollection<TCnQlog> TCnQlogs { get; set; } = new List<TCnQlog>();
}
