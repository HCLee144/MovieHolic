using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TmTypeList
{
    public int FId { get; set; }

    public int FMovieId { get; set; }

    public int FTypeId { get; set; }

    public virtual TmMovie FMovie { get; set; } = null!;

    public virtual TmType FType { get; set; } = null!;
}
