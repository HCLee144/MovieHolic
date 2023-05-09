using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TTagList
{
    public int FTagId { get; set; }

    public string FTagText { get; set; } = null!;

    public virtual ICollection<TArticleTag> TArticleTags { get; set; } = new List<TArticleTag>();
}
