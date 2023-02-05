using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AceJobAgency.ViewModels
{
    public class Register
    {
        [Required]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }
        [Required]
		[DataType(DataType.Text)]
		public string LastName { get; set; }
        [Required]
		[DataType(DataType.Text)]
		public string Gender { get; set; }
        [Required]
		[DataType(DataType.Text)]
		public string NRIC { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, MinLength(12, ErrorMessage ="please enter 12 character")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation password does not match")]
        public string ConfirmPassword { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [DataType(DataType.Upload)]
        public string Resume { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string WhoamI { get; set; }

        /*        [MaxLength(50)]
                public string? FileURL { get; set; }*/
    }
}
