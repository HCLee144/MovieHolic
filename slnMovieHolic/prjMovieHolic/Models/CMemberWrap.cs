namespace prjMovieHolic.Models
{
    public class CMemberWrap
    {
        private TMember _member;
        public TMember member
        {
            get { return _member; }
            set { _member = value; }
        }
        public CMemberWrap()
        {
            _member = new TMember();
        }
        public int FMemberId { get { return _member.FMemberId; } set { _member.FMemberId = value; } }
        public string FName { get { return _member.FName; } set { _member.FName = value; } }
        public string FIdcardNumber { get { return _member.FIdcardNumber; } set { _member.FIdcardNumber = value; } }
        public string FPassword { get { return _member.FPassword; } set { _member.FPassword = value; } }
        public string? FEmail { get { return _member.FEmail; } set { _member.FEmail = value; } }
        
        public byte FMembershipId { get { return _member.FMembershipId; } set { _member.FMembershipId = value; } }

        public string FPhone { get { return _member.FPhone; } set { _member.FPhone = value; } }

        public DateTime? FBirthDate { get { return _member.FBirthDate; } set { _member.FBirthDate = value; } }

        public int FGenderId { get { return _member.FGenderId; } set { _member.FGenderId = value; } }

        public string? FNickname { get { return _member.FNickname; } set { _member.FNickname = value; } }
        public DateTime FCreatedDate { get { return _member.FCreatedDate; } set { _member.FCreatedDate = value; } }

        public TGender FGender { get; set; } = null!;

        public TMembership FMembership { get; set; }

        public bool IsLogin { get; set; }

    }
}

