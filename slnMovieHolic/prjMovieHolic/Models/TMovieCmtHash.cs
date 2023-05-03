using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TMovieCmtHash
{
    public int FCmthashId { get; set; }

    public int FMovieCmtId { get; set; }

    public int FHashtagId { get; set; }

    public virtual THashtag FHashtag { get; set; } = null!;
}
