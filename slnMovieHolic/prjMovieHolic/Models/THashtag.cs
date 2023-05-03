using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class THashtag
{
    public int FHashtagId { get; set; }

    public string FText { get; set; } = null!;

    public virtual ICollection<TMovieCmtHash> TMovieCmtHashes { get; set; } = new List<TMovieCmtHash>();
}
