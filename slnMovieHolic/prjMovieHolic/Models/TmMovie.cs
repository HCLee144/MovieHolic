using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TmMovie
{
    public int FMovieId { get; set; }

    public int? FSeriesId { get; set; }

    public string FNameCht { get; set; } = null!;

    public string FNameEng { get; set; } = null!;

    public DateTime FScheduleStart { get; set; }

    public DateTime FScheduleEnd { get; set; }

    public int? FShowLength { get; set; }

    public string? FInteroduce { get; set; }

    public string? FTrailerLink { get; set; }

    public string? FPosterPath { get; set; }

    public decimal? FPrice { get; set; }

    public virtual TmSeries? FSeries { get; set; }

    public virtual ICollection<MemberAction> MemberActions { get; set; } = new List<MemberAction>();

    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();

    public virtual ICollection<TMovieCmt> TMovieCmts { get; set; } = new List<TMovieCmt>();

    public virtual ICollection<TmActorList> TmActorLists { get; set; } = new List<TmActorList>();

    public virtual ICollection<TmCountryList> TmCountryLists { get; set; } = new List<TmCountryList>();

    public virtual ICollection<TmDirectorList> TmDirectorLists { get; set; } = new List<TmDirectorList>();

    public virtual ICollection<TmImageList> TmImageLists { get; set; } = new List<TmImageList>();

    public virtual ICollection<TmLanguageList> TmLanguageLists { get; set; } = new List<TmLanguageList>();

    public virtual ICollection<TmTypeList> TmTypeLists { get; set; } = new List<TmTypeList>();
}
