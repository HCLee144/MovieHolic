using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace prjMovieHolic.Models;

public partial class MovieContext : DbContext
{
    public MovieContext()
    {
    }

    public MovieContext(DbContextOptions<MovieContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActionType> ActionTypes { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CouponList> CouponLists { get; set; }

    public virtual DbSet<CouponType> CouponTypes { get; set; }

    public virtual DbSet<CreditCardType> CreditCardTypes { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<MemberAction> MemberActions { get; set; }

    public virtual DbSet<Membership> Memberships { get; set; }

    public virtual DbSet<MembershipChangeLog> MembershipChangeLogs { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<OrderStatusLog> OrderStatusLogs { get; set; }

    public virtual DbSet<PayType> PayTypes { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Receipt> Receipts { get; set; }

    public virtual DbSet<ReceiptDetail> ReceiptDetails { get; set; }

    public virtual DbSet<Seat> Seats { get; set; }

    public virtual DbSet<SeatStatus> SeatStatuses { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<TCmtImage> TCmtImages { get; set; }

    public virtual DbSet<TCnQ> TCnQs { get; set; }

    public virtual DbSet<TCnQlog> TCnQlogs { get; set; }

    public virtual DbSet<TCnQtype> TCnQtypes { get; set; }

    public virtual DbSet<TFaq> TFaqs { get; set; }

    public virtual DbSet<THashtag> THashtags { get; set; }

    public virtual DbSet<TMovieCmt> TMovieCmts { get; set; }

    public virtual DbSet<TMovieCmtHash> TMovieCmtHashes { get; set; }

    public virtual DbSet<TReply> TReplies { get; set; }

    public virtual DbSet<TStatusType> TStatusTypes { get; set; }

    public virtual DbSet<Theater> Theaters { get; set; }

    public virtual DbSet<TheaterClass> TheaterClasses { get; set; }

    public virtual DbSet<TicketClass> TicketClasses { get; set; }

    public virtual DbSet<TmActor> TmActors { get; set; }

    public virtual DbSet<TmActorList> TmActorLists { get; set; }

    public virtual DbSet<TmCountry> TmCountries { get; set; }

    public virtual DbSet<TmCountryList> TmCountryLists { get; set; }

    public virtual DbSet<TmDirector> TmDirectors { get; set; }

    public virtual DbSet<TmDirectorList> TmDirectorLists { get; set; }

    public virtual DbSet<TmImage> TmImages { get; set; }

    public virtual DbSet<TmImageList> TmImageLists { get; set; }

    public virtual DbSet<TmLanguage> TmLanguages { get; set; }

    public virtual DbSet<TmLanguageList> TmLanguageLists { get; set; }

    public virtual DbSet<TmMovie> TmMovies { get; set; }

    public virtual DbSet<TmSeries> TmSeries { get; set; }

    public virtual DbSet<TmType> TmTypes { get; set; }

    public virtual DbSet<TmTypeList> TmTypeLists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=NeilLiuTW.asuscomm.com, 1433; Initial Catalog=Movie; User ID=movieTeam; Password=m2023Team; TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActionType>(entity =>
        {
            entity.ToTable("ActionType", "Members");

            entity.Property(e => e.ActionTypeId)
                .ValueGeneratedOnAdd()
                .HasColumnName("ActionTypeID");
            entity.Property(e => e.ActionTypeName)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category", "Products");

            entity.Property(e => e.CategoryId).HasColumnName("Category_ID");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(50)
                .HasColumnName("Category_Name");
        });

        modelBuilder.Entity<CouponList>(entity =>
        {
            entity.HasKey(e => e.CouponId);

            entity.ToTable("CouponList", "Products");

            entity.Property(e => e.CouponId).HasColumnName("Coupon_ID");
            entity.Property(e => e.CouponTypeId).HasColumnName("CouponType_ID");
            entity.Property(e => e.MemberId).HasColumnName("Member_ID");
            entity.Property(e => e.OrderId).HasColumnName("Order_ID");
            entity.Property(e => e.ReceiveDate).HasColumnType("date");

            entity.HasOne(d => d.CouponType).WithMany(p => p.CouponLists)
                .HasForeignKey(d => d.CouponTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CouponList_Coupon");

            entity.HasOne(d => d.Member).WithMany(p => p.CouponLists)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CouponList_Member");
        });

        modelBuilder.Entity<CouponType>(entity =>
        {
            entity.HasKey(e => e.CouponTypeId).HasName("PK_Coupon");

            entity.ToTable("CouponType", "Products");

            entity.Property(e => e.CouponTypeId).HasColumnName("CouponType_ID");
            entity.Property(e => e.CouponDiscount).HasColumnName("Coupon_Discount");
            entity.Property(e => e.CouponDueDate)
                .HasColumnType("date")
                .HasColumnName("Coupon_DueDate");
            entity.Property(e => e.CouponStartDate)
                .HasColumnType("date")
                .HasColumnName("Coupon_StartDate");
            entity.Property(e => e.CouponTypeName)
                .HasMaxLength(50)
                .HasColumnName("CouponType_Name");
        });

        modelBuilder.Entity<CreditCardType>(entity =>
        {
            entity.HasKey(e => e.CreditCardTypeId).HasName("PK_CreditCard");

            entity.ToTable("CreditCardType", "Order");

            entity.Property(e => e.CreditCardTypeId).HasColumnName("CreditCardType_ID");
            entity.Property(e => e.CreditCardType1)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CreditCardType");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employee", "Employee");

            entity.Property(e => e.EmployeeId).HasColumnName("Employee_ID");
            entity.Property(e => e.EmployeeAccount).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.ToTable("Gender", "Members");

            entity.Property(e => e.GenderId).HasColumnName("GenderID");
            entity.Property(e => e.GenderName)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("Gender_Name");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.ToTable("Member", "Members");

            entity.Property(e => e.MemberId)
                .ValueGeneratedNever()
                .HasColumnName("MemberID");
            entity.Property(e => e.BirthDate).HasColumnType("date");
            entity.Property(e => e.CreatedDate).HasColumnType("date");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.GenderId).HasColumnName("GenderID");
            entity.Property(e => e.IdcardNumber)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("IDCardNumber");
            entity.Property(e => e.MembershipId).HasColumnName("MembershipID");
            entity.Property(e => e.Name).HasMaxLength(10);
            entity.Property(e => e.Nickname).HasMaxLength(10);
            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Gender).WithMany(p => p.Members)
                .HasForeignKey(d => d.GenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Member_Gender");

            entity.HasOne(d => d.Membership).WithMany(p => p.Members)
                .HasForeignKey(d => d.MembershipId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Member_Membership");
        });

        modelBuilder.Entity<MemberAction>(entity =>
        {
            entity.ToTable("MemberAction", "Members");

            entity.Property(e => e.MemberActionId).HasColumnName("MemberActionID");
            entity.Property(e => e.ActionTypeId).HasColumnName("ActionTypeID");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.MovieId).HasColumnName("MovieID");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.ActionType).WithMany(p => p.MemberActions)
                .HasForeignKey(d => d.ActionTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MemberAction_ActionType");

            entity.HasOne(d => d.Member).WithMany(p => p.MemberActions)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MemberAction_Member1");

            entity.HasOne(d => d.Movie).WithMany(p => p.MemberActions)
                .HasForeignKey(d => d.MovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MemberAction_Movie");
        });

        modelBuilder.Entity<Membership>(entity =>
        {
            entity.ToTable("Membership", "Members");

            entity.Property(e => e.MembershipId)
                .ValueGeneratedOnAdd()
                .HasColumnName("MembershipID");
            entity.Property(e => e.MembershipName)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("Membership_Name");
            entity.Property(e => e.PriceRate).HasColumnType("decimal(3, 2)");
        });

        modelBuilder.Entity<MembershipChangeLog>(entity =>
        {
            entity.ToTable("MembershipChangeLog", "Members");

            entity.Property(e => e.MembershipChangeLogId).HasColumnName("MembershipChangeLogID");
            entity.Property(e => e.ChangeDate).HasColumnType("datetime");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.MembershipId).HasColumnName("MembershipID");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order", "Order");

            entity.Property(e => e.OrderId).HasColumnName("Order_ID");
            entity.Property(e => e.CouponId).HasColumnName("Coupon_ID");
            entity.Property(e => e.CreditCardTypeId).HasColumnName("CreditCardType_ID");
            entity.Property(e => e.MemberId).HasColumnName("Member_ID");
            entity.Property(e => e.OrderDate).HasColumnType("smalldatetime");
            entity.Property(e => e.PayTypeId).HasColumnName("PayType_ID");
            entity.Property(e => e.SessionId).HasColumnName("Session_ID");

            entity.HasOne(d => d.CreditCardType).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CreditCardTypeId)
                .HasConstraintName("FK_Order_CreditCardType");

            entity.HasOne(d => d.Member).WithMany(p => p.Orders)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Member");

            entity.HasOne(d => d.PayType).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PayTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_PayType");

            entity.HasOne(d => d.Session).WithMany(p => p.Orders)
                .HasForeignKey(d => d.SessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Session");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.ToTable("OrderDetail", "Order");

            entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetail_ID");
            entity.Property(e => e.OrderId).HasColumnName("Order_ID");
            entity.Property(e => e.SeatId).HasColumnName("Seat_ID");
            entity.Property(e => e.TicketClassId).HasColumnName("TicketClass_ID");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_Order");

            entity.HasOne(d => d.Seat).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.SeatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_Seat");

            entity.HasOne(d => d.TicketClass).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.TicketClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_TicketClass");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.ToTable("OrderStatus", "Order");

            entity.Property(e => e.OrderStatusId).HasColumnName("OrderStatus_ID");
            entity.Property(e => e.ChangedTime).HasColumnType("smalldatetime");
            entity.Property(e => e.OrderStatus1)
                .HasMaxLength(15)
                .HasColumnName("OrderStatus");
        });

        modelBuilder.Entity<OrderStatusLog>(entity =>
        {
            entity.ToTable("OrderStatusLog", "Order");

            entity.Property(e => e.OrderStatusLogId).HasColumnName("OrderStatusLog_ID");
            entity.Property(e => e.OrderId).HasColumnName("Order_ID");
            entity.Property(e => e.OrderStatusId).HasColumnName("OrderStatus_ID");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderStatusLogs)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderStatusLog_Order");

            entity.HasOne(d => d.OrderStatus).WithMany(p => p.OrderStatusLogs)
                .HasForeignKey(d => d.OrderStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderStatusLog_OrderStatus");
        });

        modelBuilder.Entity<PayType>(entity =>
        {
            entity.ToTable("PayType", "Order");

            entity.Property(e => e.PayTypeId).HasColumnName("PayType_ID");
            entity.Property(e => e.PayType1)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("PayType");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK_Products");

            entity.ToTable("Product", "Products");

            entity.Property(e => e.ProductId)
                .ValueGeneratedNever()
                .HasColumnName("Product_ID");
            entity.Property(e => e.CategoryId).HasColumnName("Category_ID");
            entity.Property(e => e.Image).HasColumnType("image");
            entity.Property(e => e.ImagePath).HasMaxLength(50);
            entity.Property(e => e.Introduce).HasMaxLength(50);
            entity.Property(e => e.ProductName).HasMaxLength(50);

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Category");
        });

        modelBuilder.Entity<Receipt>(entity =>
        {
            entity.ToTable("Receipt", "Products");

            entity.Property(e => e.ReceiptId).HasColumnName("Receipt_ID");
            entity.Property(e => e.MemberId).HasColumnName("Member_ID");
            entity.Property(e => e.OrderId).HasColumnName("Order_ID");
            entity.Property(e => e.ReceiptDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");

            entity.HasOne(d => d.Member).WithMany(p => p.Receipts)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Receipt_Member");
        });

        modelBuilder.Entity<ReceiptDetail>(entity =>
        {
            entity.ToTable("ReceiptDetail", "Products");

            entity.Property(e => e.ReceiptDetailId).HasColumnName("ReceiptDetail_ID");
            entity.Property(e => e.ProductId).HasColumnName("Product_ID");
            entity.Property(e => e.ReceiptId).HasColumnName("Receipt_ID");

            entity.HasOne(d => d.Product).WithMany(p => p.ReceiptDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReceiptDetail_Products");

            entity.HasOne(d => d.Receipt).WithMany(p => p.ReceiptDetails)
                .HasForeignKey(d => d.ReceiptId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReceiptDetail_Receipt");
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.ToTable("Seat", "Theater");

            entity.Property(e => e.SeatId).HasColumnName("Seat_ID");
            entity.Property(e => e.SeatStatusId).HasColumnName("SeatStatus_ID");
            entity.Property(e => e.TheaterId).HasColumnName("Theater_ID");

            entity.HasOne(d => d.SeatStatus).WithMany(p => p.Seats)
                .HasForeignKey(d => d.SeatStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Seat_SeatStatus");

            entity.HasOne(d => d.Theater).WithMany(p => p.Seats)
                .HasForeignKey(d => d.TheaterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Seat_Theater");
        });

        modelBuilder.Entity<SeatStatus>(entity =>
        {
            entity.ToTable("SeatStatus", "Theater");

            entity.HasIndex(e => e.SeatStatusId, "IX_SeatStatus").IsUnique();

            entity.Property(e => e.SeatStatusId)
                .ValueGeneratedNever()
                .HasColumnName("SeatStatus_ID");
            entity.Property(e => e.SeatStatus1)
                .HasMaxLength(10)
                .HasColumnName("SeatStatus");
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.ToTable("Session", "Theater");

            entity.Property(e => e.SessionId).HasColumnName("Session_ID");
            entity.Property(e => e.EndTime).HasColumnType("smalldatetime");
            entity.Property(e => e.MovieId).HasColumnName("Movie_ID");
            entity.Property(e => e.StartTime).HasColumnType("smalldatetime");
            entity.Property(e => e.TheaterId).HasColumnName("Theater_ID");

            entity.HasOne(d => d.Movie).WithMany(p => p.Sessions)
                .HasForeignKey(d => d.MovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Session_Movie");

            entity.HasOne(d => d.Theater).WithMany(p => p.Sessions)
                .HasForeignKey(d => d.TheaterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Session_Theater");
        });

        modelBuilder.Entity<TCmtImage>(entity =>
        {
            entity.HasKey(e => e.FCmtImgId).HasName("PK_Complaint_ImageList");

            entity.ToTable("tCmtImage", "CMT");

            entity.Property(e => e.FCmtImgId).HasColumnName("fCmtImgID");
            entity.Property(e => e.FCmtId).HasColumnName("fCmt_ID");
            entity.Property(e => e.FImgName)
                .HasMaxLength(50)
                .HasColumnName("fImgName");

            entity.HasOne(d => d.FCmt).WithMany(p => p.TCmtImages)
                .HasForeignKey(d => d.FCmtId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cmt_Image__Movie__61BB7BD9");
        });

        modelBuilder.Entity<TCnQ>(entity =>
        {
            entity.HasKey(e => e.FCnQid).HasName("PK_Conplaints");

            entity.ToTable("tCnQ", "CMT");

            entity.Property(e => e.FCnQid).HasColumnName("fCnQID");
            entity.Property(e => e.FCnQtypeId).HasColumnName("fCnQType_ID");
            entity.Property(e => e.FIsComplaint).HasColumnName("fIsComplaint");
            entity.Property(e => e.FMemberId).HasColumnName("fMember_ID");
            entity.Property(e => e.FText)
                .HasColumnType("ntext")
                .HasColumnName("fText");

            entity.HasOne(d => d.FCnQtype).WithMany(p => p.TCnQs)
                .HasForeignKey(d => d.FCnQtypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Conplaints_ComplaintTypes");

            entity.HasOne(d => d.FMember).WithMany(p => p.TCnQs)
                .HasForeignKey(d => d.FMemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CnQs__Member_ID__60C757A0");
        });

        modelBuilder.Entity<TCnQlog>(entity =>
        {
            entity.HasKey(e => e.FCnQlogId).HasName("PK_ComplaintStatusList");

            entity.ToTable("tCnQLog", "CMT");

            entity.Property(e => e.FCnQlogId).HasColumnName("fCnQLogID");
            entity.Property(e => e.FCnQId).HasColumnName("fCnQ_ID");
            entity.Property(e => e.FReplyId).HasColumnName("fReply_ID");
            entity.Property(e => e.FStatusId).HasColumnName("fStatus_ID");
            entity.Property(e => e.FTimeStamp)
                .HasColumnType("datetime")
                .HasColumnName("fTimeStamp");

            entity.HasOne(d => d.FCnQ).WithMany(p => p.TCnQlogs)
                .HasForeignKey(d => d.FCnQId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ComplaintStatusList_ConplaintsAndQs");

            entity.HasOne(d => d.FReply).WithMany(p => p.TCnQlogs)
                .HasForeignKey(d => d.FReplyId)
                .HasConstraintName("FK_ComplaintStatusList_Replies");

            entity.HasOne(d => d.FStatus).WithMany(p => p.TCnQlogs)
                .HasForeignKey(d => d.FStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ComplaintStatusList_StatusList");
        });

        modelBuilder.Entity<TCnQtype>(entity =>
        {
            entity.HasKey(e => e.FCnQtypeId).HasName("PK_ComplaintTypes");

            entity.ToTable("tCnQTypes", "CMT");

            entity.Property(e => e.FCnQtypeId)
                .ValueGeneratedNever()
                .HasColumnName("fCnQTypeID");
            entity.Property(e => e.FCnQtype)
                .HasMaxLength(20)
                .HasColumnName("fCnQType");
        });

        modelBuilder.Entity<TFaq>(entity =>
        {
            entity.HasKey(e => e.FFaqid).HasName("PK_FAQ");

            entity.ToTable("tFAQ", "CMT");

            entity.Property(e => e.FFaqid).HasColumnName("fFAQID");
            entity.Property(e => e.FAnswer)
                .HasColumnType("ntext")
                .HasColumnName("fAnswer");
            entity.Property(e => e.FCnQtypeId).HasColumnName("fCnQType_ID");
            entity.Property(e => e.FFaq)
                .HasMaxLength(100)
                .HasColumnName("fFAQ");

            entity.HasOne(d => d.FCnQtype).WithMany(p => p.TFaqs)
                .HasForeignKey(d => d.FCnQtypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FAQ_CnQTypes");
        });

        modelBuilder.Entity<THashtag>(entity =>
        {
            entity.HasKey(e => e.FHashtagId).HasName("PK_Hashtags");

            entity.ToTable("tHashtag", "CMT");

            entity.Property(e => e.FHashtagId).HasColumnName("fHashtagID");
            entity.Property(e => e.FText)
                .HasMaxLength(20)
                .HasColumnName("fText");
        });

        modelBuilder.Entity<TMovieCmt>(entity =>
        {
            entity.HasKey(e => e.FCmtid).HasName("PK_MovieComments");

            entity.ToTable("tMovieCmt", "CMT");

            entity.Property(e => e.FCmtid).HasColumnName("fCMTID");
            entity.Property(e => e.FCreatedTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fCreatedTime");
            entity.Property(e => e.FEditedTime)
                .HasColumnType("datetime")
                .HasColumnName("fEditedTime");
            entity.Property(e => e.FMemberId).HasColumnName("fMember_ID");
            entity.Property(e => e.FMovieId).HasColumnName("fMovie_ID");
            entity.Property(e => e.FRate).HasColumnName("fRate");
            entity.Property(e => e.FText)
                .HasColumnType("text")
                .HasColumnName("fText");
            entity.Property(e => e.FTitle)
                .HasMaxLength(20)
                .HasColumnName("fTitle");
            entity.Property(e => e.FVisible)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasColumnName("fVisible");

            entity.HasOne(d => d.FMember).WithMany(p => p.TMovieCmts)
                .HasForeignKey(d => d.FMemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MovieCmts__Membe__5FD33367");

            entity.HasOne(d => d.FMovie).WithMany(p => p.TMovieCmts)
                .HasForeignKey(d => d.FMovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovieCmts_Movie");
        });

        modelBuilder.Entity<TMovieCmtHash>(entity =>
        {
            entity.HasKey(e => e.FCmthashId).HasName("PK_Com_Hash");

            entity.ToTable("tMovieCmt_Hash", "CMT");

            entity.Property(e => e.FCmthashId).HasColumnName("fCMTHashID");
            entity.Property(e => e.FHashtagId).HasColumnName("fHashtagID");
            entity.Property(e => e.FMovieCmtId).HasColumnName("fMovieCmtID");

            entity.HasOne(d => d.FHashtag).WithMany(p => p.TMovieCmtHashes)
                .HasForeignKey(d => d.FHashtagId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Com_Hash_Hashtags");
        });

        modelBuilder.Entity<TReply>(entity =>
        {
            entity.HasKey(e => e.FReplyId).HasName("PK_Replies");

            entity.ToTable("tReply", "CMT");

            entity.Property(e => e.FReplyId).HasColumnName("fReplyID");
            entity.Property(e => e.FReply)
                .HasMaxLength(300)
                .HasColumnName("fReply");
        });

        modelBuilder.Entity<TStatusType>(entity =>
        {
            entity.HasKey(e => e.FStatusTypeId).HasName("PK_StatusList");

            entity.ToTable("tStatusType", "CMT");

            entity.Property(e => e.FStatusTypeId).HasColumnName("fStatusTypeID");
            entity.Property(e => e.FStatusText)
                .HasMaxLength(20)
                .HasColumnName("fStatusText");
        });

        modelBuilder.Entity<Theater>(entity =>
        {
            entity.ToTable("Theater", "Theater");

            entity.Property(e => e.TheaterId)
                .ValueGeneratedNever()
                .HasColumnName("Theater_ID");
            entity.Property(e => e.Theater1)
                .HasMaxLength(20)
                .HasColumnName("Theater");
            entity.Property(e => e.TheaterClassId).HasColumnName("TheaterClass_ID");

            entity.HasOne(d => d.TheaterClass).WithMany(p => p.Theaters)
                .HasForeignKey(d => d.TheaterClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Theater_TheaterClass");
        });

        modelBuilder.Entity<TheaterClass>(entity =>
        {
            entity.ToTable("TheaterClass", "Theater");

            entity.Property(e => e.TheaterClassId).HasColumnName("TheaterClass_ID");
            entity.Property(e => e.PriceRate).HasColumnType("decimal(3, 2)");
            entity.Property(e => e.TheaterClass1)
                .HasMaxLength(10)
                .HasColumnName("TheaterClass");
        });

        modelBuilder.Entity<TicketClass>(entity =>
        {
            entity.HasKey(e => e.TicketClassId).HasName("PK_Ticket_Class");

            entity.ToTable("TicketClass", "Theater");

            entity.Property(e => e.TicketClassId).HasColumnName("TicketClass_ID");
            entity.Property(e => e.PriceRate).HasColumnType("decimal(3, 2)");
            entity.Property(e => e.TicketClass1)
                .HasMaxLength(10)
                .HasColumnName("TicketClass");
        });

        modelBuilder.Entity<TmActor>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK_Actors");

            entity.ToTable("tmActor", "Movie");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FActorImagePath)
                .HasMaxLength(50)
                .HasColumnName("fActorImagePath");
            entity.Property(e => e.FActorNameCht)
                .HasMaxLength(50)
                .HasColumnName("fActorNameCht");
            entity.Property(e => e.FActorNameEng)
                .HasMaxLength(50)
                .HasColumnName("fActorNameEng");
        });

        modelBuilder.Entity<TmActorList>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK_Case");

            entity.ToTable("tmActorList", "Movie");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FActorId).HasColumnName("fActorId");
            entity.Property(e => e.FCharactorNameCht)
                .HasMaxLength(50)
                .HasColumnName("fCharactorNameCht");
            entity.Property(e => e.FMovieId).HasColumnName("fMovieId");

            entity.HasOne(d => d.FActor).WithMany(p => p.TmActorLists)
                .HasForeignKey(d => d.FActorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Case_Actors");

            entity.HasOne(d => d.FMovie).WithMany(p => p.TmActorLists)
                .HasForeignKey(d => d.FMovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Case_Movie");
        });

        modelBuilder.Entity<TmCountry>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK_Country");

            entity.ToTable("tmCountry", "Movie");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FImagePath)
                .HasMaxLength(50)
                .HasColumnName("fImagePath");
            entity.Property(e => e.FNameCht)
                .HasMaxLength(50)
                .HasColumnName("fNameCht");
            entity.Property(e => e.FNameEng)
                .HasMaxLength(50)
                .HasColumnName("fNameEng");
        });

        modelBuilder.Entity<TmCountryList>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK_MovieOrigin");

            entity.ToTable("tmCountryList", "Movie");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FCountryId).HasColumnName("fCountryId");
            entity.Property(e => e.FMovieId).HasColumnName("fMovieId");

            entity.HasOne(d => d.FCountry).WithMany(p => p.TmCountryLists)
                .HasForeignKey(d => d.FCountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovieOrigin_Country");

            entity.HasOne(d => d.FMovie).WithMany(p => p.TmCountryLists)
                .HasForeignKey(d => d.FMovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovieCountriesList_Movies");
        });

        modelBuilder.Entity<TmDirector>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK_Directors");

            entity.ToTable("tmDirector", "Movie");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FImagePath).HasColumnName("fImagePath");
            entity.Property(e => e.FNameCht)
                .HasMaxLength(50)
                .HasColumnName("fNameCht");
            entity.Property(e => e.FNameEng)
                .HasMaxLength(50)
                .HasColumnName("fNameEng");
        });

        modelBuilder.Entity<TmDirectorList>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK_MovieDirectors");

            entity.ToTable("tmDirectorList", "Movie");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FDirectorId).HasColumnName("fDirectorId");
            entity.Property(e => e.FMovieId).HasColumnName("fMovieId");

            entity.HasOne(d => d.FDirector).WithMany(p => p.TmDirectorLists)
                .HasForeignKey(d => d.FDirectorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovieDirectorsList_Directors");

            entity.HasOne(d => d.FMovie).WithMany(p => p.TmDirectorLists)
                .HasForeignKey(d => d.FMovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovieDirectors_Movie");
        });

        modelBuilder.Entity<TmImage>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK_MovieImage");

            entity.ToTable("tmImage", "Movie");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FImagePath)
                .HasMaxLength(50)
                .HasColumnName("fImagePath");
        });

        modelBuilder.Entity<TmImageList>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK_MovieImageList");

            entity.ToTable("tmImageList", "Movie");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FImageId).HasColumnName("fImageId");
            entity.Property(e => e.FMovieId).HasColumnName("fMovieId");

            entity.HasOne(d => d.FImage).WithMany(p => p.TmImageLists)
                .HasForeignKey(d => d.FImageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovieImageList_MovieImage");

            entity.HasOne(d => d.FMovie).WithMany(p => p.TmImageLists)
                .HasForeignKey(d => d.FMovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovieImagesList_Movies");
        });

        modelBuilder.Entity<TmLanguage>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK_MovieLanguage");

            entity.ToTable("tmLanguage", "Movie");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FNameCht)
                .HasMaxLength(50)
                .HasColumnName("fNameCht");
        });

        modelBuilder.Entity<TmLanguageList>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK_MovieCode");

            entity.ToTable("tmLanguageList", "Movie");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FLanguageId).HasColumnName("fLanguageId");
            entity.Property(e => e.FMovieId).HasColumnName("fMovieId");

            entity.HasOne(d => d.FLanguage).WithMany(p => p.TmLanguageLists)
                .HasForeignKey(d => d.FLanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovieCode_MovieLanguage");

            entity.HasOne(d => d.FMovie).WithMany(p => p.TmLanguageLists)
                .HasForeignKey(d => d.FMovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovieLanguagesList_Movies");
        });

        modelBuilder.Entity<TmMovie>(entity =>
        {
            entity.HasKey(e => e.FMovieId).HasName("PK_Movie");

            entity.ToTable("tmMovie", "Movie");

            entity.Property(e => e.FMovieId)
                .ValueGeneratedNever()
                .HasColumnName("fMovieId");
            entity.Property(e => e.FInteroduce).HasColumnName("fInteroduce");
            entity.Property(e => e.FNameCht)
                .HasMaxLength(50)
                .HasColumnName("fNameCht");
            entity.Property(e => e.FNameEng)
                .HasMaxLength(50)
                .HasColumnName("fNameEng");
            entity.Property(e => e.FPosterPath)
                .HasMaxLength(50)
                .HasColumnName("fPosterPath");
            entity.Property(e => e.FPrice)
                .HasColumnType("money")
                .HasColumnName("fPrice");
            entity.Property(e => e.FScheduleEnd)
                .HasColumnType("smalldatetime")
                .HasColumnName("fScheduleEnd");
            entity.Property(e => e.FScheduleStart)
                .HasColumnType("smalldatetime")
                .HasColumnName("fScheduleStart");
            entity.Property(e => e.FSeriesId).HasColumnName("fSeriesId");
            entity.Property(e => e.FShowLength).HasColumnName("fShowLength");
            entity.Property(e => e.FTrailerLink).HasColumnName("fTrailerLink");

            entity.HasOne(d => d.FSeries).WithMany(p => p.TmMovies)
                .HasForeignKey(d => d.FSeriesId)
                .HasConstraintName("FK_Movie_MovieSeries");
        });

        modelBuilder.Entity<TmSeries>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK_MovieSeries");

            entity.ToTable("tmSeries", "Movie");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FNameCht)
                .HasMaxLength(50)
                .HasColumnName("fNameCht");
        });

        modelBuilder.Entity<TmType>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK_MovieTypeList");

            entity.ToTable("tmType", "Movie");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FNameCht)
                .HasMaxLength(50)
                .HasColumnName("fNameCht");
        });

        modelBuilder.Entity<TmTypeList>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK_MovieType");

            entity.ToTable("tmTypeList", "Movie");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FMovieId).HasColumnName("fMovieId");
            entity.Property(e => e.FTypeId).HasColumnName("fTypeId");

            entity.HasOne(d => d.FMovie).WithMany(p => p.TmTypeLists)
                .HasForeignKey(d => d.FMovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovieType_Movie");

            entity.HasOne(d => d.FType).WithMany(p => p.TmTypeLists)
                .HasForeignKey(d => d.FTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovieType_MovieTypeList");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
