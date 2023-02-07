using AuthService.Models.DBModels;
using AuthService.Models.Enum; 

namespace AuthService.Models.Dtos
{
    public class EmailDto
    {
        public string EmailAddress { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public string Role { get; set; }
        public EmailTemplate EmailType { get; set; }
        public string SiteUrl { get; set; }
        public string ActivationLink { get; set; }
        public ApplicationUser User { get; set; }
        public bool IsResetPassword { get; set; }

    }
}
