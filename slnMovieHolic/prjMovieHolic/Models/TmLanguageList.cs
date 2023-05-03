using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TmLanguageList
{
    public int FId { get; set; }

    public int FMovieId { get; set; }

    public int FLanguageId { get; set; }

    public virtual TmLanguage FLanguage { get; set; } = null!;

    public virtual TmMovie FMovie { get; set; } = null!;
}
