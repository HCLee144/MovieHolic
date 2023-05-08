using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TCategory
{
    public int FCategoryId { get; set; }

    public string? FCategoryName { get; set; }

    public virtual ICollection<TProduct> TProducts { get; set; } = new List<TProduct>();
}
