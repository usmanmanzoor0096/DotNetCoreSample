using System.Security.Claims;

namespace AuthService.Models.ResponseModel
{
    public class LoginResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Id { get; set; }
        public string WalletAddress { get; set; }
        public List<Claim>? Claims { get; set; }
        public bool IsPrivacyPolicyAccepted { get; set; }
        public string UserProfileAvatar { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
