using AuthService.Models.Enum;
using Newtonsoft.Json.Converters;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AuthService.Models.RequestModels
{
    public class TokenRequest
    {
        public string GrantType { get; set; }
        public string Email { get; set; }
        public string UserUuId { get; set; }
        public string Password { get; set; }
        public string RefreshToken { get; set; }
        public string ClientId { get; set; }
        public bool RememberMe { get; set; }
    }
    public class LoginRequest
    {
        [Required]
        [EnumDataType(typeof(GrandType))]
        //[JsonConverter(typeof(StringEnumConverter))]
        public GrandType GrantType { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }
        //[Required]
        //public string UserId { get; set; }

        [DefaultValue(false)]
        public bool RememberMe { get; set; }
    }

    public class LoginRequestWithRefreshToken
    {
        [Required]
        public GrandType GrantType { get; set; }
        [Required]
        public string RefreshToken { get; set; }
        [Required]
        public string UserUuId { get; set; } 
    }

}
