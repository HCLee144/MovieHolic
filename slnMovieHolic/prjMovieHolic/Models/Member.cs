using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class Member
{
    public int MemberId { get; set; }

    public string IdcardNumber { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public byte MembershipId { get; set; }

    public string Phone { get; set; } = null!;

    public DateTime? BirthDate { get; set; }

    public int GenderId { get; set; }

    public string? Nickname { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<CouponList> CouponLists { get; set; } = new List<CouponList>();

    public virtual Gender Gender { get; set; } = null!;

    public virtual ICollection<MemberAction> MemberActions { get; set; } = new List<MemberAction>();

    public virtual Membership Membership { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();

    public virtual ICollection<TCnQ> TCnQs { get; set; } = new List<TCnQ>();

    public virtual ICollection<TMovieCmt> TMovieCmts { get; set; } = new List<TMovieCmt>();
}
