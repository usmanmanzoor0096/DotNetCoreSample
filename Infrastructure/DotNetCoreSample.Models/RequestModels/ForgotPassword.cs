using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.RequestModels
{
    public class ForgotPassword
    {
        public string Email { get; set; }
        public string? ClientUri { get; set; }
    }

    public class UserRequestModel
    {
        public string Email { get; set; }
    }
}
