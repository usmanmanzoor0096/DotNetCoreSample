using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace  AuthService.Common.Utils
{
    public class UtilService : IUtilService
    {
        public string GenerateMD5Hash(string sessionId)
        {
            // Step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(sessionId);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }
        public string GetRemoteIPAddress(HttpContext context, bool allowForwarded = true)
        {
            if (allowForwarded)
            {
                string header = (context.Request.Headers["CF-Connecting-IP"].FirstOrDefault() ?? context.Request.Headers["X-Forwarded-For"].FirstOrDefault());
                if (IPAddress.TryParse(header, out IPAddress ip))
                {
                    return ip.MapToIPv4().ToString();
                }
            }
            return context.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
        public string GenerateTwoFACode()
        {
            Random RNG = new Random();
            string code = "";
            for (var i = 0; i < 6; i++)
            {
                code += ((int)(RNG.Next(1, 9))).ToString().ToLower();
            }
            return code;
        }
    }
}
