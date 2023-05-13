using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TArticleImg
{
    public int FimgId { get; set; }

    public string FimagePath { get; set; } = null!;

    public string FblockId { get; set; } = null!;
}
