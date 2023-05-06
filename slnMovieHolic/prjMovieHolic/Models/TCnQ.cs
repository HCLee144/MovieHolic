using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TCnQ
{
    public int FCnQid { get; set; }

    public int FCnQtypeId { get; set; }

    public int FMemberId { get; set; }

    public bool FIsComplaint { get; set; }

    public string FText { get; set; } = null!;

    public virtual TCnQtype FCnQtype { get; set; } = null!;

    public virtual TMember FMember { get; set; } = null!;

    public virtual ICollection<TCnQlog> TCnQlogs { get; set; } = new List<TCnQlog>();
}
