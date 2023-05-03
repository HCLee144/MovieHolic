using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class Gender
{
    public int GenderId { get; set; }

    public string GenderName { get; set; } = null!;

    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
}
