using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TGender
{
    public int FGenderId { get; set; }

    public string FGenderName { get; set; } = null!;

    public virtual ICollection<TMember> TMembers { get; set; } = new List<TMember>();
}
