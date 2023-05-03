using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TmType
{
    public int FId { get; set; }

    public string FNameCht { get; set; } = null!;

    public virtual ICollection<TmTypeList> TmTypeLists { get; set; } = new List<TmTypeList>();
}
