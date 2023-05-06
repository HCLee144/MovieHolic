using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TTheater
{
    public int FTheaterId { get; set; }

    public string FTheater { get; set; } = null!;

    public int FTheaterClassId { get; set; }

    public virtual TTheaterClass FTheaterClass { get; set; } = null!;

    public virtual ICollection<TSeat> TSeats { get; set; } = new List<TSeat>();

    public virtual ICollection<TSession> TSessions { get; set; } = new List<TSession>();
}
