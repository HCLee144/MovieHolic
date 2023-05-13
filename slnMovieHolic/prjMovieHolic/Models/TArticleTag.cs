using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TArticleTag
{
    public int FId { get; set; }

    public int FArticleId { get; set; }

    public int FTagId { get; set; }

    public virtual TArticle FArticle { get; set; } = null!;

    public virtual TTagList FTag { get; set; } = null!;
}
