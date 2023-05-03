using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TCnQtype
{
    public int FCnQtypeId { get; set; }

    public string FCnQtype { get; set; } = null!;

    public virtual ICollection<TCnQ> TCnQs { get; set; } = new List<TCnQ>();

    public virtual ICollection<TFaq> TFaqs { get; set; } = new List<TFaq>();
}
