using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TLanguage
{
    public int FId { get; set; }

    public string? FNameCht { get; set; }

    public string FNameEng { get; set; } = null!;

    public virtual ICollection<TLanguageList> TLanguageLists { get; set; } = new List<TLanguageList>();
}
