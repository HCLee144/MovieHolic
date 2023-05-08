using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TEmployee
{
    public int FEmployeeId { get; set; }

    public string FEmployeeAccount { get; set; } = null!;

    public string FPassword { get; set; } = null!;

    public int FAccess { get; set; }
}
