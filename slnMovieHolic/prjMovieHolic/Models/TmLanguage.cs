using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TmLanguage
{
    public int FId { get; set; }

    public string FNameCht { get; set; } = null!;

    public virtual ICollection<TmLanguageList> TmLanguageLists { get; set; } = new List<TmLanguageList>();
}
