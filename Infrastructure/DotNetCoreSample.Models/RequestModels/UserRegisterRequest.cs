using AuthService.Models.Enum;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace AuthService.Models.RequestModels
{
    public class UserRegisterRequest
    {
        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "UserEmail is required")]
        [Display(Name = "UserEmail")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string? UserEmail { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Client id is required")]
        [Display(Name = "Client Id")]
        public string? ClientId { get; set; }

        //[System.Text.Json.Serialization.JsonIgnore]
        [JsonIgnore]
        public Roles UserRole { get; set; }

        [DefaultValue(false)]
        public bool RememberMe { get; set; }

    }
 
}
