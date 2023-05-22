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

    public virtual DbSet<TActionType> TActionTypes { get; set; }

    public virtual DbSet<TActor> TActors { get; set; }

    public virtual DbSet<TActorList> TActorLists { get; set; }

    public virtual DbSet<TArticle> TArticles { get; set; }

    public virtual DbSet<TArticleImg> TArticleImgs { get; set; }

    public virtual DbSet<TArticleTag> TArticleTags { get; set; }

    public virtual DbSet<TCategory> TCategories { get; set; }

    public virtual DbSet<TCnQ> TCnQs { get; set; }

    public virtual DbSet<TCnQlog> TCnQlogs { get; set; }

    public virtual DbSet<TCnQtype> TCnQtypes { get; set; }

    public virtual DbSet<TCountry> TCountries { get; set; }

    public virtual DbSet<TCountryList> TCountryLists { get; set; }

    public virtual DbSet<TCouponList> TCouponLists { get; set; }

    public virtual DbSet<TCouponType> TCouponTypes { get; set; }

    public virtual DbSet<TCreditCardType> TCreditCardTypes { get; set; }

    public virtual DbSet<TDirector> TDirectors { get; set; }

    public virtual DbSet<TDirectorList> TDirectorLists { get; set; }

    public virtual DbSet<TEmployee> TEmployees { get; set; }

    public virtual DbSet<TFaq> TFaqs { get; set; }

    public virtual DbSet<TGender> TGenders { get; set; }

    public virtual DbSet<TLanguage> TLanguages { get; set; }

    public virtual DbSet<TLanguageList> TLanguageLists { get; set; }

    public virtual DbSet<TMember> TMembers { get; set; }

    public virtual DbSet<TMemberAction> TMemberActions { get; set; }

    public virtual DbSet<TMembership> TMemberships { get; set; }

    public virtual DbSet<TMembershipChangeLog> TMembershipChangeLogs { get; set; }

    public virtual DbSet<TMovie> TMovies { get; set; }

    public virtual DbSet<TOrder> TOrders { get; set; }

    public virtual DbSet<TOrderDetail> TOrderDetails { get; set; }

    public virtual DbSet<TOrderStatus> TOrderStatuses { get; set; }

    public virtual DbSet<TOrderStatusLog> TOrderStatusLogs { get; set; }

    public virtual DbSet<TPayType> TPayTypes { get; set; }

    public virtual DbSet<TProduct> TProducts { get; set; }

    public virtual DbSet<TRating> TRatings { get; set; }

    public virtual DbSet<TReceipt> TReceipts { get; set; }

    public virtual DbSet<TReceiptDetail> TReceiptDetails { get; set; }

    public virtual DbSet<TReply> TReplies { get; set; }

    public virtual DbSet<TSeat> TSeats { get; set; }

    public virtual DbSet<TSeatStatus> TSeatStatuses { get; set; }

    public virtual DbSet<TSeries> TSeries { get; set; }

    public virtual DbSet<TSession> TSessions { get; set; }

    public virtual DbSet<TShortCmt> TShortCmts { get; set; }

    public virtual DbSet<TStatusType> TStatusTypes { get; set; }

    public virtual DbSet<TTagList> TTagLists { get; set; }

    public virtual DbSet<TTheater> TTheaters { get; set; }

    public virtual DbSet<TTheaterClass> TTheaterClasses { get; set; }

    public virtual DbSet<TTicketClass> TTicketClasses { get; set; }

    public virtual DbSet<TType> TTypes { get; set; }

    public virtual DbSet<TTypeList> TTypeLists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=NeilLiuTW.asuscomm.com, 1433; Initial Catalog=Movie; User ID=movieTeam; Password=m2023Team; TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TActionType>(entity =>
        {
            entity.HasKey(e => e.FActionTypeId).HasName("PK_ActionType");

            entity.ToTable("tActionType", "Members");

            entity.Property(e => e.FActionTypeId)
                .ValueGeneratedOnAdd()
                .HasColumnName("fActionTypeID");
            entity.Property(e => e.FActionTypeName)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("fActionTypeName");
        });

        modelBuilder.Entity<TActor>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK_Actors");

            entity.ToTable("tActor", "Movie");

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

        modelBuilder.Entity<TActorList>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK_Case");

            entity.ToTable("tActorList", "Movie");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FActorId).HasColumnName("fActorId");
            entity.Property(e => e.FCharactorNameCht)
                .HasMaxLength(50)
                .HasColumnName("fCharactorNameCht");
            entity.Property(e => e.FMovieId).HasColumnName("fMovieId");

            entity.HasOne(d => d.FActor).WithMany(p => p.TActorLists)
                .HasForeignKey(d => d.FActorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Case_Actors");

            entity.HasOne(d => d.FMovie).WithMany(p => p.TActorLists)
                .HasForeignKey(d => d.FMovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Case_Movie");
        });

        modelBuilder.Entity<TArticle>(entity =>
        {
            entity.HasKey(e => e.FArticleId);

            entity.ToTable("tArticle", "CMT");

            entity.Property(e => e.FArticleId).HasColumnName("fArticleID");
            entity.Property(e => e.FBlockJson).HasColumnName("fBlockJson");
            entity.Property(e => e.FIsPublic).HasColumnName("fIsPublic");
            entity.Property(e => e.FMemberId).HasColumnName("fMemberID");
            entity.Property(e => e.FMovieId).HasColumnName("fMovieID");
            entity.Property(e => e.FTimeCreated)
                .HasColumnType("datetime")
                .HasColumnName("fTimeCreated");
            entity.Property(e => e.FTimeEdited)
                .HasColumnType("datetime")
                .HasColumnName("fTimeEdited");
            entity.Property(e => e.FTitle)
                .HasMaxLength(50)
                .HasColumnName("fTitle");

            entity.HasOne(d => d.FMember).WithMany(p => p.TArticles)
                .HasForeignKey(d => d.FMemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tArticle_tMember");

            entity.HasOne(d => d.FMovie).WithMany(p => p.TArticles)
                .HasForeignKey(d => d.FMovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tArticle_tMovie");
        });

        modelBuilder.Entity<TArticleImg>(entity =>
        {
            entity.HasKey(e => e.FimgId);

            entity.ToTable("tArticleImg", "CMT");

            entity.Property(e => e.FimgId).HasColumnName("FImgID");
            entity.Property(e => e.FblockId)
                .HasMaxLength(50)
                .HasColumnName("FBlockID");
            entity.Property(e => e.FimagePath)
                .HasMaxLength(50)
                .HasColumnName("FImagePath");
        });

        modelBuilder.Entity<TArticleTag>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK_Com_Hash");

            entity.ToTable("tArticleTags", "CMT");

            entity.Property(e => e.FId).HasColumnName("fID");
            entity.Property(e => e.FArticleId).HasColumnName("fArticleID");
            entity.Property(e => e.FTagId).HasColumnName("fTagID");

            entity.HasOne(d => d.FArticle).WithMany(p => p.TArticleTags)
                .HasForeignKey(d => d.FArticleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tArticleTags_tArticle");

            entity.HasOne(d => d.FTag).WithMany(p => p.TArticleTags)
                .HasForeignKey(d => d.FTagId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Com_Hash_Hashtags");
        });

        modelBuilder.Entity<TCategory>(entity =>
        {
            entity.HasKey(e => e.FCategoryId).HasName("PK_Category");

            entity.ToTable("tCategory", "Products");

            entity.Property(e => e.FCategoryId).HasColumnName("fCategory_ID");
            entity.Property(e => e.FCategoryName)
                .HasMaxLength(50)
                .HasColumnName("fCategory_Name");
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

        modelBuilder.Entity<TCountry>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK_Country");

            entity.ToTable("tCountry", "Movie");

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

        modelBuilder.Entity<TCountryList>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK_MovieOrigin");

            entity.ToTable("tCountryList", "Movie");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FCountryId).HasColumnName("fCountryId");
            entity.Property(e => e.FMovieId).HasColumnName("fMovieId");

            entity.HasOne(d => d.FCountry).WithMany(p => p.TCountryLists)
                .HasForeignKey(d => d.FCountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovieOrigin_Country");

            entity.HasOne(d => d.FMovie).WithMany(p => p.TCountryLists)
                .HasForeignKey(d => d.FMovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovieCountriesList_Movies");
        });

        modelBuilder.Entity<TCouponList>(entity =>
        {
            entity.HasKey(e => e.FCouponId).HasName("PK_CouponList");

            entity.ToTable("tCouponList", "Products");

            entity.Property(e => e.FCouponId).HasColumnName("fCoupon_ID");
            entity.Property(e => e.FCouponTypeId).HasColumnName("fCouponType_ID");
            entity.Property(e => e.FIsUsed).HasColumnName("fIsUsed");
            entity.Property(e => e.FMemberId).HasColumnName("fMember_ID");
            entity.Property(e => e.FOrderId).HasColumnName("fOrder_ID");
            entity.Property(e => e.FReceiveDate)
                .HasColumnType("date")
                .HasColumnName("fReceiveDate");

            entity.HasOne(d => d.FCouponType).WithMany(p => p.TCouponLists)
                .HasForeignKey(d => d.FCouponTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CouponList_Coupon");

            entity.HasOne(d => d.FMember).WithMany(p => p.TCouponLists)
                .HasForeignKey(d => d.FMemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CouponList_Member");
        });

        modelBuilder.Entity<TCouponType>(entity =>
        {
            entity.HasKey(e => e.FCouponTypeId).HasName("PK_Coupon");

            entity.ToTable("tCouponType", "Products");

            entity.Property(e => e.FCouponTypeId).HasColumnName("fCouponType_ID");
            entity.Property(e => e.FCouponDiscount).HasColumnName("fCoupon_Discount");
            entity.Property(e => e.FCouponDueDate)
                .HasColumnType("date")
                .HasColumnName("fCoupon_DueDate");
            entity.Property(e => e.FCouponStartDate)
                .HasColumnType("date")
                .HasColumnName("fCoupon_StartDate");
            entity.Property(e => e.FCouponTypeName)
                .HasMaxLength(50)
                .HasColumnName("fCouponType_Name");
        });

        modelBuilder.Entity<TCreditCardType>(entity =>
        {
            entity.HasKey(e => e.FCreditCardTypeId).HasName("PK_CreditCard");

            entity.ToTable("tCreditCardType", "Order");

            entity.Property(e => e.FCreditCardTypeId).HasColumnName("fCreditCardType_ID");
            entity.Property(e => e.FCreditCardType)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("fCreditCardType");
            entity.Property(e => e.FPriceRate).HasColumnName("fPriceRate");
        });

        modelBuilder.Entity<TDirector>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK_Directors");

            entity.ToTable("tDirector", "Movie");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FImagePath).HasColumnName("fImagePath");
            entity.Property(e => e.FNameCht)
                .HasMaxLength(50)
                .HasColumnName("fNameCht");
            entity.Property(e => e.FNameEng)
                .HasMaxLength(50)
                .HasColumnName("fNameEng");
        });

        modelBuilder.Entity<TDirectorList>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK_MovieDirectors");

            entity.ToTable("tDirectorList", "Movie");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FDirectorId).HasColumnName("fDirectorId");
            entity.Property(e => e.FMovieId).HasColumnName("fMovieId");

            entity.HasOne(d => d.FDirector).WithMany(p => p.TDirectorLists)
                .HasForeignKey(d => d.FDirectorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovieDirectorsList_Directors");

            entity.HasOne(d => d.FMovie).WithMany(p => p.TDirectorLists)
                .HasForeignKey(d => d.FMovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovieDirectors_Movie");
        });

        modelBuilder.Entity<TEmployee>(entity =>
        {
            entity.HasKey(e => e.FEmployeeId).HasName("PK_Employee");

            entity.ToTable("tEmployee", "Employee");

            entity.Property(e => e.FEmployeeId).HasColumnName("fEmployee_ID");
            entity.Property(e => e.FAccess).HasColumnName("fAccess");
            entity.Property(e => e.FEmployeeAccount)
                .HasMaxLength(50)
                .HasColumnName("fEmployeeAccount");
            entity.Property(e => e.FPassword)
                .HasMaxLength(50)
                .HasColumnName("fPassword");
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

        modelBuilder.Entity<TGender>(entity =>
        {
            entity.HasKey(e => e.FGenderId).HasName("PK_Gender");

            entity.ToTable("tGender", "Members");

            entity.Property(e => e.FGenderId).HasColumnName("fGenderID");
            entity.Property(e => e.FGenderName)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("fGender_Name");
        });

        modelBuilder.Entity<TLanguage>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK_MovieLanguage");

            entity.ToTable("tLanguage", "Movie");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FNameCht)
                .HasMaxLength(50)
                .HasColumnName("fNameCht");
            entity.Property(e => e.FNameEng)
                .HasMaxLength(50)
                .HasColumnName("fNameEng");
        });

        modelBuilder.Entity<TLanguageList>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK_MovieCode");

            entity.ToTable("tLanguageList", "Movie");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FLanguageId).HasColumnName("fLanguageId");
            entity.Property(e => e.FMovieId).HasColumnName("fMovieId");

            entity.HasOne(d => d.FLanguage).WithMany(p => p.TLanguageLists)
                .HasForeignKey(d => d.FLanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovieCode_MovieLanguage");

            entity.HasOne(d => d.FMovie).WithMany(p => p.TLanguageLists)
                .HasForeignKey(d => d.FMovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovieLanguagesList_Movies");
        });

        modelBuilder.Entity<TMember>(entity =>
        {
            entity.HasKey(e => e.FMemberId).HasName("PK_Member");

            entity.ToTable("tMember", "Members");

            entity.Property(e => e.FMemberId).HasColumnName("fMemberID");
            entity.Property(e => e.FBirthDate)
                .HasColumnType("date")
                .HasColumnName("fBirthDate");
            entity.Property(e => e.FCreatedDate)
                .HasColumnType("date")
                .HasColumnName("fCreatedDate");
            entity.Property(e => e.FEmail)
                .HasMaxLength(50)
                .HasColumnName("fEmail");
            entity.Property(e => e.FGenderId).HasColumnName("fGenderID");
            entity.Property(e => e.FIdcardNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("fIDCardNumber");
            entity.Property(e => e.FMembershipId).HasColumnName("fMembershipID");
            entity.Property(e => e.FName)
                .HasMaxLength(10)
                .HasColumnName("fName");
            entity.Property(e => e.FNickname)
                .HasMaxLength(10)
                .HasColumnName("fNickname");
            entity.Property(e => e.FPassword)
                .IsUnicode(false)
                .HasColumnName("fPassword");
            entity.Property(e => e.FPhone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("fPhone");

            entity.HasOne(d => d.FGender).WithMany(p => p.TMembers)
                .HasForeignKey(d => d.FGenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Member_Gender");

            entity.HasOne(d => d.FMembership).WithMany(p => p.TMembers)
                .HasForeignKey(d => d.FMembershipId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Member_Membership");
        });

        modelBuilder.Entity<TMemberAction>(entity =>
        {
            entity.HasKey(e => e.FMemberActionId).HasName("PK_MemberAction");

            entity.ToTable("tMemberAction", "Members");

            entity.Property(e => e.FMemberActionId).HasColumnName("fMemberActionID");
            entity.Property(e => e.FActionTypeId).HasColumnName("fActionTypeID");
            entity.Property(e => e.FMemberId).HasColumnName("fMemberID");
            entity.Property(e => e.FMovieId).HasColumnName("fMovieID");
            entity.Property(e => e.FTimeStamp)
                .HasColumnType("datetime")
                .HasColumnName("fTimeStamp");

            entity.HasOne(d => d.FActionType).WithMany(p => p.TMemberActions)
                .HasForeignKey(d => d.FActionTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MemberAction_ActionType");

            entity.HasOne(d => d.FMember).WithMany(p => p.TMemberActions)
                .HasForeignKey(d => d.FMemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MemberAction_Member1");

            entity.HasOne(d => d.FMovie).WithMany(p => p.TMemberActions)
                .HasForeignKey(d => d.FMovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MemberAction_Movie");
        });

        modelBuilder.Entity<TMembership>(entity =>
        {
            entity.HasKey(e => e.FMembershipId).HasName("PK_Membership");

            entity.ToTable("tMembership", "Members");

            entity.Property(e => e.FMembershipId)
                .ValueGeneratedOnAdd()
                .HasColumnName("fMembershipID");
            entity.Property(e => e.FMembershipName)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("fMembership_Name");
            entity.Property(e => e.FPriceRate)
                .HasColumnType("decimal(3, 2)")
                .HasColumnName("fPriceRate");
        });

        modelBuilder.Entity<TMembershipChangeLog>(entity =>
        {
            entity.HasKey(e => e.FMembershipChangeLogId).HasName("PK_MembershipChangeLog");

            entity.ToTable("tMembershipChangeLog", "Members");

            entity.Property(e => e.FMembershipChangeLogId).HasColumnName("fMembershipChangeLogID");
            entity.Property(e => e.FChangeDate)
                .HasColumnType("datetime")
                .HasColumnName("fChangeDate");
            entity.Property(e => e.FMemberId).HasColumnName("fMemberID");
            entity.Property(e => e.FMembershipId).HasColumnName("fMembershipID");
        });

        modelBuilder.Entity<TMovie>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK_Movie");

            entity.ToTable("tMovie", "Movie");

            entity.Property(e => e.FId)
                .ValueGeneratedNever()
                .HasColumnName("fId");
            entity.Property(e => e.FImagePath)
                .HasMaxLength(50)
                .HasColumnName("fImagePath");
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
            entity.Property(e => e.FRatingId).HasColumnName("fRatingId");
            entity.Property(e => e.FScheduleEnd)
                .HasColumnType("smalldatetime")
                .HasColumnName("fScheduleEnd");
            entity.Property(e => e.FScheduleStart)
                .HasColumnType("smalldatetime")
                .HasColumnName("fScheduleStart");
            entity.Property(e => e.FSeriesId).HasColumnName("fSeriesId");
            entity.Property(e => e.FShowLength).HasColumnName("fShowLength");
            entity.Property(e => e.FTrailerLink).HasColumnName("fTrailerLink");

            entity.HasOne(d => d.FRating).WithMany(p => p.TMovies)
                .HasForeignKey(d => d.FRatingId)
                .HasConstraintName("FK_tMovie_tRating");

            entity.HasOne(d => d.FSeries).WithMany(p => p.TMovies)
                .HasForeignKey(d => d.FSeriesId)
                .HasConstraintName("FK_Movie_MovieSeries");
        });

        modelBuilder.Entity<TOrder>(entity =>
        {
            entity.HasKey(e => e.FOrderId).HasName("PK_Order");

            entity.ToTable("tOrder", "Order");

            entity.Property(e => e.FOrderId).HasColumnName("fOrder_ID");
            entity.Property(e => e.FCouponId).HasColumnName("fCoupon_ID");
            entity.Property(e => e.FCreditCardTypeId).HasColumnName("fCreditCardType_ID");
            entity.Property(e => e.FInvoiceNumber).HasColumnName("fInvoiceNumber");
            entity.Property(e => e.FMemberId).HasColumnName("fMember_ID");
            entity.Property(e => e.FOrderDate)
                .HasColumnType("smalldatetime")
                .HasColumnName("fOrderDate");
            entity.Property(e => e.FPayTypeId).HasColumnName("fPayType_ID");
            entity.Property(e => e.FSessionId).HasColumnName("fSession_ID");
            entity.Property(e => e.FTotalPrice).HasColumnName("fTotalPrice");

            entity.HasOne(d => d.FCreditCardType).WithMany(p => p.TOrders)
                .HasForeignKey(d => d.FCreditCardTypeId)
                .HasConstraintName("FK_Order_CreditCardType");

            entity.HasOne(d => d.FMember).WithMany(p => p.TOrders)
                .HasForeignKey(d => d.FMemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Member");

            entity.HasOne(d => d.FPayType).WithMany(p => p.TOrders)
                .HasForeignKey(d => d.FPayTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_PayType");

            entity.HasOne(d => d.FSession).WithMany(p => p.TOrders)
                .HasForeignKey(d => d.FSessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Session");
        });

        modelBuilder.Entity<TOrderDetail>(entity =>
        {
            entity.HasKey(e => e.FOrderDetailId).HasName("PK_OrderDetail");

            entity.ToTable("tOrderDetail", "Order");

            entity.Property(e => e.FOrderDetailId).HasColumnName("fOrderDetail_ID");
            entity.Property(e => e.FOrderId).HasColumnName("fOrder_ID");
            entity.Property(e => e.FSeatId).HasColumnName("fSeat_ID");
            entity.Property(e => e.FTicketClassId).HasColumnName("fTicketClass_ID");

            entity.HasOne(d => d.FOrder).WithMany(p => p.TOrderDetails)
                .HasForeignKey(d => d.FOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_Order");

            entity.HasOne(d => d.FSeat).WithMany(p => p.TOrderDetails)
                .HasForeignKey(d => d.FSeatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_Seat");

            entity.HasOne(d => d.FTicketClass).WithMany(p => p.TOrderDetails)
                .HasForeignKey(d => d.FTicketClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_TicketClass");
        });

        modelBuilder.Entity<TOrderStatus>(entity =>
        {
            entity.HasKey(e => e.FOrderStatusId).HasName("PK_OrderStatus");

            entity.ToTable("tOrderStatus", "Order");

            entity.Property(e => e.FOrderStatusId).HasColumnName("fOrderStatus_ID");
            entity.Property(e => e.FChangedTime)
                .HasColumnType("smalldatetime")
                .HasColumnName("fChangedTime");
            entity.Property(e => e.FOrderStatus)
                .HasMaxLength(15)
                .HasColumnName("fOrderStatus");
        });

        modelBuilder.Entity<TOrderStatusLog>(entity =>
        {
            entity.HasKey(e => e.FOrderStatusLogId).HasName("PK_OrderStatusLog");

            entity.ToTable("tOrderStatusLog", "Order");

            entity.Property(e => e.FOrderStatusLogId).HasColumnName("fOrderStatusLog_ID");
            entity.Property(e => e.FOrderId).HasColumnName("fOrder_ID");
            entity.Property(e => e.FOrderStatusId).HasColumnName("fOrderStatus_ID");

            entity.HasOne(d => d.FOrder).WithMany(p => p.TOrderStatusLogs)
                .HasForeignKey(d => d.FOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderStatusLog_Order");

            entity.HasOne(d => d.FOrderStatus).WithMany(p => p.TOrderStatusLogs)
                .HasForeignKey(d => d.FOrderStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderStatusLog_OrderStatus");
        });

        modelBuilder.Entity<TPayType>(entity =>
        {
            entity.HasKey(e => e.FPayTypeId).HasName("PK_PayType");

            entity.ToTable("tPayType", "Order");

            entity.Property(e => e.FPayTypeId).HasColumnName("fPayType_ID");
            entity.Property(e => e.FPayType)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("fPayType");
        });

        modelBuilder.Entity<TProduct>(entity =>
        {
            entity.HasKey(e => e.FProductId).HasName("PK_Products");

            entity.ToTable("tProduct", "Products");

            entity.Property(e => e.FProductId)
                .ValueGeneratedNever()
                .HasColumnName("fProduct_ID");
            entity.Property(e => e.FCategoryId).HasColumnName("fCategory_ID");
            entity.Property(e => e.FImage)
                .HasColumnType("image")
                .HasColumnName("fImage");
            entity.Property(e => e.FImagePath)
                .HasMaxLength(50)
                .HasColumnName("fImagePath");
            entity.Property(e => e.FIntroduce)
                .HasMaxLength(50)
                .HasColumnName("fIntroduce");
            entity.Property(e => e.FProductName)
                .HasMaxLength(50)
                .HasColumnName("fProductName");
            entity.Property(e => e.FProductPrice).HasColumnName("fProductPrice");

            entity.HasOne(d => d.FCategory).WithMany(p => p.TProducts)
                .HasForeignKey(d => d.FCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Category");
        });

        modelBuilder.Entity<TRating>(entity =>
        {
            entity.HasKey(e => e.FId);

            entity.ToTable("tRating", "Movie");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FImagePath)
                .HasMaxLength(50)
                .HasColumnName("fImagePath");
            entity.Property(e => e.FNameCht)
                .HasMaxLength(50)
                .HasColumnName("fNameCht");
        });

        modelBuilder.Entity<TReceipt>(entity =>
        {
            entity.HasKey(e => e.FReceiptId).HasName("PK_Receipt");

            entity.ToTable("tReceipt", "Products");

            entity.Property(e => e.FReceiptId).HasColumnName("fReceipt_ID");
            entity.Property(e => e.FMemberId).HasColumnName("fMember_ID");
            entity.Property(e => e.FOrderId).HasColumnName("fOrder_ID");
            entity.Property(e => e.FReceiptDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime")
                .HasColumnName("fReceiptDate");

            entity.HasOne(d => d.FMember).WithMany(p => p.TReceipts)
                .HasForeignKey(d => d.FMemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Receipt_Member");
        });

        modelBuilder.Entity<TReceiptDetail>(entity =>
        {
            entity.HasKey(e => e.FReceiptDetailId).HasName("PK_ReceiptDetail");

            entity.ToTable("tReceiptDetail", "Products");

            entity.Property(e => e.FReceiptDetailId).HasColumnName("fReceiptDetail_ID");
            entity.Property(e => e.FProductId).HasColumnName("fProduct_ID");
            entity.Property(e => e.FQty).HasColumnName("fQty");
            entity.Property(e => e.FReceiptId).HasColumnName("fReceipt_ID");

            entity.HasOne(d => d.FProduct).WithMany(p => p.TReceiptDetails)
                .HasForeignKey(d => d.FProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReceiptDetail_Products");

            entity.HasOne(d => d.FReceipt).WithMany(p => p.TReceiptDetails)
                .HasForeignKey(d => d.FReceiptId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReceiptDetail_Receipt");
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

        modelBuilder.Entity<TSeat>(entity =>
        {
            entity.HasKey(e => e.FSeatId).HasName("PK_Seat");

            entity.ToTable("tSeat", "Theater");

            entity.Property(e => e.FSeatId).HasColumnName("fSeat_ID");
            entity.Property(e => e.FSeatNum).HasColumnName("fSeatNum");
            entity.Property(e => e.FSeatRow).HasColumnName("fSeatRow");
            entity.Property(e => e.FSeatStatusId).HasColumnName("fSeatStatus_ID");
            entity.Property(e => e.FTheaterId).HasColumnName("fTheater_ID");

            entity.HasOne(d => d.FSeatStatus).WithMany(p => p.TSeats)
                .HasForeignKey(d => d.FSeatStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Seat_SeatStatus");

            entity.HasOne(d => d.FTheater).WithMany(p => p.TSeats)
                .HasForeignKey(d => d.FTheaterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Seat_Theater");
        });

        modelBuilder.Entity<TSeatStatus>(entity =>
        {
            entity.HasKey(e => e.FSeatStatusId).HasName("PK_SeatStatus");

            entity.ToTable("tSeatStatus", "Theater");

            entity.HasIndex(e => e.FSeatStatusId, "IX_SeatStatus").IsUnique();

            entity.Property(e => e.FSeatStatusId)
                .ValueGeneratedNever()
                .HasColumnName("fSeatStatus_ID");
            entity.Property(e => e.FSeatStatus)
                .HasMaxLength(10)
                .HasColumnName("fSeatStatus");
        });

        modelBuilder.Entity<TSeries>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK_MovieSeries");

            entity.ToTable("tSeries", "Movie");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FNameCht)
                .HasMaxLength(50)
                .HasColumnName("fNameCht");
        });

        modelBuilder.Entity<TSession>(entity =>
        {
            entity.HasKey(e => e.FSessionId).HasName("PK_Session");

            entity.ToTable("tSession", "Theater");

            entity.Property(e => e.FSessionId).HasColumnName("fSession_ID");
            entity.Property(e => e.FEndTime)
                .HasColumnType("smalldatetime")
                .HasColumnName("fEndTime");
            entity.Property(e => e.FMovieId).HasColumnName("fMovie_ID");
            entity.Property(e => e.FStartTime)
                .HasColumnType("smalldatetime")
                .HasColumnName("fStartTime");
            entity.Property(e => e.FTheaterId).HasColumnName("fTheater_ID");

            entity.HasOne(d => d.FMovie).WithMany(p => p.TSessions)
                .HasForeignKey(d => d.FMovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Session_Movie");

            entity.HasOne(d => d.FTheater).WithMany(p => p.TSessions)
                .HasForeignKey(d => d.FTheaterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Session_Theater");
        });

        modelBuilder.Entity<TShortCmt>(entity =>
        {
            entity.HasKey(e => e.FCmtid).HasName("PK_MovieComments");

            entity.ToTable("tShortCmt", "CMT");

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
            entity.Property(e => e.FTitle)
                .HasMaxLength(50)
                .HasColumnName("fTitle");
            entity.Property(e => e.FVisible)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasColumnName("fVisible");

            entity.HasOne(d => d.FMember).WithMany(p => p.TShortCmts)
                .HasForeignKey(d => d.FMemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MovieCmts__Membe__5FD33367");

            entity.HasOne(d => d.FMovie).WithMany(p => p.TShortCmts)
                .HasForeignKey(d => d.FMovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovieCmts_Movie");
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

        modelBuilder.Entity<TTagList>(entity =>
        {
            entity.HasKey(e => e.FTagId).HasName("PK_Hashtags");

            entity.ToTable("tTagList", "CMT");

            entity.Property(e => e.FTagId).HasColumnName("fTagID");
            entity.Property(e => e.FTagText)
                .HasMaxLength(20)
                .HasColumnName("fTagText");
        });

        modelBuilder.Entity<TTheater>(entity =>
        {
            entity.HasKey(e => e.FTheaterId).HasName("PK_Theater");

            entity.ToTable("tTheater", "Theater");

            entity.Property(e => e.FTheaterId)
                .ValueGeneratedNever()
                .HasColumnName("fTheater_ID");
            entity.Property(e => e.FTheater)
                .HasMaxLength(20)
                .HasColumnName("fTheater");
            entity.Property(e => e.FTheaterClassId).HasColumnName("fTheaterClass_ID");

            entity.HasOne(d => d.FTheaterClass).WithMany(p => p.TTheaters)
                .HasForeignKey(d => d.FTheaterClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Theater_TheaterClass");
        });

        modelBuilder.Entity<TTheaterClass>(entity =>
        {
            entity.HasKey(e => e.FTheaterClassId).HasName("PK_TheaterClass");

            entity.ToTable("tTheaterClass", "Theater");

            entity.Property(e => e.FTheaterClassId).HasColumnName("fTheaterClass_ID");
            entity.Property(e => e.FPriceRate)
                .HasColumnType("decimal(3, 2)")
                .HasColumnName("fPriceRate");
            entity.Property(e => e.FTheaterClass)
                .HasMaxLength(10)
                .HasColumnName("fTheaterClass");
        });

        modelBuilder.Entity<TTicketClass>(entity =>
        {
            entity.HasKey(e => e.FTicketClassId).HasName("PK_Ticket_Class");

            entity.ToTable("tTicketClass", "Theater");

            entity.Property(e => e.FTicketClassId).HasColumnName("fTicketClass_ID");
            entity.Property(e => e.FPriceRate)
                .HasColumnType("decimal(3, 2)")
                .HasColumnName("fPriceRate");
            entity.Property(e => e.FTicketClass)
                .HasMaxLength(10)
                .HasColumnName("fTicketClass");
        });

        modelBuilder.Entity<TType>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK_MovieTypeList");

            entity.ToTable("tType", "Movie");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FNameCht)
                .HasMaxLength(50)
                .HasColumnName("fNameCht");
        });

        modelBuilder.Entity<TTypeList>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK_MovieType");

            entity.ToTable("tTypeList", "Movie");

            entity.Property(e => e.FId)
                .ValueGeneratedNever()
                .HasColumnName("fId");
            entity.Property(e => e.FMovieId).HasColumnName("fMovieId");
            entity.Property(e => e.FTypeId).HasColumnName("fTypeId");

            entity.HasOne(d => d.FMovie).WithMany(p => p.TTypeLists)
                .HasForeignKey(d => d.FMovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovieType_Movie");

            entity.HasOne(d => d.FType).WithMany(p => p.TTypeLists)
                .HasForeignKey(d => d.FTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovieType_MovieTypeList");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
