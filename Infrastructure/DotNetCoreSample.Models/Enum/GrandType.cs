
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AuthService.Models.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum GrandType
    {
        [Description("Password")]
        Password = 1,
        [Description("AccessToken")]
        AccessToken = 2,
        [Description("RefreshToken")]
        RefreshToken = 3,
        TwoFa = 4
    }
}
