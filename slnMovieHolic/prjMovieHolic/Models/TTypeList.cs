using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TTypeList
{
    public int FId { get; set; }

    public int FMovieId { get; set; }

    public int FTypeId { get; set; }

    public virtual TMovie FMovie { get; set; } = null!;

    public virtual TType FType { get; set; } = null!;
}
