namespace prjMovieHolic.ViewModels
{
    public class CSignUpViewModel
    {
        public string FIdcardNumber { get; set; } 

        public string FPassword { get; set; } 
        public string FPasswordCheck { get; set; }

        public string FName { get; set; } 

        public string FEmail { get; set; } 

        public byte FMembershipId { get; set; }

        public string FPhone { get; set; } 

        public DateTime? FBirthDate { get; set; }

        public int FGenderId { get; set; }

        public string? FNickname { get; set; }

        public DateTime FCreatedDate { get; set; }
    }
}
