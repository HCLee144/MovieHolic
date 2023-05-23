using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TArtFollow
{
    public int FollowId { get; set; }

    public int ArticleId { get; set; }

    public int MemberId { get; set; }

    public virtual TArticle Article { get; set; } = null!;

    public virtual TMember Member { get; set; } = null!;
}
