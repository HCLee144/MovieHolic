using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TSeatStatus
{
    public int FSeatStatusId { get; set; }

    public string FSeatStatus { get; set; } = null!;

    public virtual ICollection<TSeat> TSeats { get; set; } = new List<TSeat>();
}
