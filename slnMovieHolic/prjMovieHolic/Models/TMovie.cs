using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TMovie
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

    public virtual TSeries? FSeries { get; set; }

    public virtual ICollection<TActorList> TActorLists { get; set; } = new List<TActorList>();

    public virtual ICollection<TCountryList> TCountryLists { get; set; } = new List<TCountryList>();

    public virtual ICollection<TDirectorList> TDirectorLists { get; set; } = new List<TDirectorList>();

    public virtual ICollection<TImageList> TImageLists { get; set; } = new List<TImageList>();

    public virtual ICollection<TLanguageList> TLanguageLists { get; set; } = new List<TLanguageList>();

    public virtual ICollection<TMemberAction> TMemberActions { get; set; } = new List<TMemberAction>();

    public virtual ICollection<TMovieCmt> TMovieCmts { get; set; } = new List<TMovieCmt>();

    public virtual ICollection<TSession> TSessions { get; set; } = new List<TSession>();

    public virtual ICollection<TTypeList> TTypeLists { get; set; } = new List<TTypeList>();
}
