using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TLanguageList
{
    public int FId { get; set; }

    public int FMovieId { get; set; }

    public int FLanguageId { get; set; }

    public virtual TLanguage FLanguage { get; set; } = null!;

    public virtual TMovie FMovie { get; set; } = null!;
}
