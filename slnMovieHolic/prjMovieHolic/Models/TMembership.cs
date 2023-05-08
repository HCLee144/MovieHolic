using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TMembership
{
    public byte FMembershipId { get; set; }

    public string FMembershipName { get; set; } = null!;

    public decimal? FPriceRate { get; set; }

    public virtual ICollection<TMember> TMembers { get; set; } = new List<TMember>();
}
