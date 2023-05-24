using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TArticle
{
    public int FArticleId { get; set; }

    public int FMemberId { get; set; }

    public int FMovieId { get; set; }

    public int? FScore { get; set; }

    public string FTitle { get; set; } = null!;

    public DateTime FTimeCreated { get; set; }

    public DateTime? FTimeEdited { get; set; }

    public string? FBlockJson { get; set; }

    public bool FIsPublic { get; set; }

    public virtual TMember FMember { get; set; } = null!;

    public virtual TMovie FMovie { get; set; } = null!;

    public virtual ICollection<TArtActList> TArtActLists { get; set; } = new List<TArtActList>();

    public virtual ICollection<TArtFollow> TArtFollows { get; set; } = new List<TArtFollow>();

    public virtual ICollection<TArticleTag> TArticleTags { get; set; } = new List<TArticleTag>();
}
