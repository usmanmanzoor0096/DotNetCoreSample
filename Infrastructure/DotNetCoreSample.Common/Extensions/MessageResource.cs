
using AuthService.Common.Resources;
using static AuthService.Common.Enums.APIEnums;

namespace AuthService.Common.Extensions
{
    public static class MessageResource
    {
        public static string GetMessage(this APIStatusCodes code)
        {
            return ApiMessagesResource.ResourceManager.GetString(code.ToString());
        }
    }
}
