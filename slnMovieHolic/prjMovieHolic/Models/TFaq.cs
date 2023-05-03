using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TFaq
{
    public int FFaqid { get; set; }

    public int FCnQtypeId { get; set; }

    public string FFaq { get; set; } = null!;

    public string FAnswer { get; set; } = null!;

    public virtual TCnQtype FCnQtype { get; set; } = null!;
}
