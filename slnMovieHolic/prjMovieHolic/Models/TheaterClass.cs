using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TheaterClass
{
    public int TheaterClassId { get; set; }

    public string TheaterClass1 { get; set; } = null!;

    public decimal PriceRate { get; set; }

    public virtual ICollection<Theater> Theaters { get; set; } = new List<Theater>();
}
