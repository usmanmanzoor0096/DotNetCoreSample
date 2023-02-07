using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.RequestModels
{
    public class SetPassword
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Code { get; set; }

        [Required]
        [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public string? ClientUri { get; set; }
    }
}
