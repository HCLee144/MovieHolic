using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TCnQlog
{
    public int FCnQlogId { get; set; }

    public int FCnQId { get; set; }

    public int FStatusId { get; set; }

    public DateTime FTimeStamp { get; set; }

    public int? FReplyId { get; set; }

    public virtual TCnQ FCnQ { get; set; } = null!;

    public virtual TReply? FReply { get; set; }

    public virtual TStatusType FStatus { get; set; } = null!;
}
