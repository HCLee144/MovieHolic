using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TType
{
    public int FId { get; set; }

    public string FNameCht { get; set; } = null!;

    public virtual ICollection<TTypeList> TTypeLists { get; set; } = new List<TTypeList>();
}
