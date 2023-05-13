using System;
using System.Collections.Generic;

namespace prjMovieHolic.Models;

public partial class TMember
{
    public int FMemberId { get; set; }

    public string FIdcardNumber { get; set; } = null!;

    public string FPassword { get; set; } = null!;

    public string FName { get; set; } = null!;

    public string FEmail { get; set; } = null!;

    public byte FMembershipId { get; set; }

    public string FPhone { get; set; } = null!;

    public DateTime? FBirthDate { get; set; }

    public int FGenderId { get; set; }

    public string? FNickname { get; set; }

    public DateTime FCreatedDate { get; set; }

    public virtual TGender FGender { get; set; } = null!;

    public virtual TMembership FMembership { get; set; } = null!;

    public virtual ICollection<TArticle> TArticles { get; set; } = new List<TArticle>();

    public virtual ICollection<TCnQ> TCnQs { get; set; } = new List<TCnQ>();

    public virtual ICollection<TCouponList> TCouponLists { get; set; } = new List<TCouponList>();

    public virtual ICollection<TMemberAction> TMemberActions { get; set; } = new List<TMemberAction>();

    public virtual ICollection<TOrder> TOrders { get; set; } = new List<TOrder>();

    public virtual ICollection<TReceipt> TReceipts { get; set; } = new List<TReceipt>();

    public virtual ICollection<TShortCmt> TShortCmts { get; set; } = new List<TShortCmt>();
}
