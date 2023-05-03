using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TCmtImage
{
    public int FCmtImgId { get; set; }

    public string FImgName { get; set; } = null!;

    public int FCmtId { get; set; }

    public virtual TMovieCmt FCmt { get; set; } = null!;
}
