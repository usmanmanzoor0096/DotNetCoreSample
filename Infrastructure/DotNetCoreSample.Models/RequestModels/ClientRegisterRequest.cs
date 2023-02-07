using System.ComponentModel.DataAnnotations; 

namespace AuthService.Models.RequestModels
{
    public class ClientRegisterRequest
    {
        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string? UserEmail { get; set; }

        //[Required(ErrorMessage = "User name is required")]
        //[Display(Name = "User Name")]
        //public string? UserName { get; set; }
        //public string Password { get; set; }
        //public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Client id is required")]
        [Display(Name = "Client Id")]
        public string? ClientId { get; set; }

        //[JsonIgnore]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //[Required(ErrorMessage = "Please select user role")]
        //[Display(Name = "User Role")]
        //[DefaultValue(Role.CLIENT)]
        //public Role UserRole { get; set; }
    }
}
