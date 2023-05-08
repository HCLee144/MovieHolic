using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TTheaterClass
{
    public int FTheaterClassId { get; set; }

    public string FTheaterClass { get; set; } = null!;

    public decimal FPriceRate { get; set; }

    public virtual ICollection<TTheater> TTheaters { get; set; } = new List<TTheater>();
}
