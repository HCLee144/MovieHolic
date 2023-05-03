using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TReply
{
    public int FReplyId { get; set; }

    public string FReply { get; set; } = null!;

    public virtual ICollection<TCnQlog> TCnQlogs { get; set; } = new List<TCnQlog>();
}
