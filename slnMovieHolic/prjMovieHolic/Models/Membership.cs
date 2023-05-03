using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class Membership
{
    public byte MembershipId { get; set; }

    public string MembershipName { get; set; } = null!;

    public decimal? PriceRate { get; set; }

    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
}
