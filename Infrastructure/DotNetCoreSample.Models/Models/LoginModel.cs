using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.Models
{
    public class LoginModel
    {
        public LoginModel()
        {
            GrantType = "password";
            DeviceInfo = new DeviceInfo();
        }
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string GrantType { get; set; }

        public string RefreshToken { get; set; }

        [Required]
        public string ClientId { get; set; }

        public DeviceInfo DeviceInfo { get; private set; }
    }

    public class DeviceInfo
    {
        public DeviceInfo()
        {
            IP = "";
            Browser = "";
            Device = "";
            Platform = "";
        }
        public string IP { get; set; }
        public string Browser { get; set; }
        public string Device { get; set; }
        public string Platform { get; set; }
    }
}
