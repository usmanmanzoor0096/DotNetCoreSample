using AuthService.Models.Config;
using AuthService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization; 
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt; 
using System.Text; 

namespace DotNetCoreSample.Auth.Service.Helper
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AuthOptions _options;
        public JwtMiddleware(RequestDelegate next, IOptions<AuthOptions> options)
        {
            _next = next;
            _options = options.Value;
        }

        public async Task Invoke(HttpContext context, IUserAuthService userAuthService)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() is object)
            {
                await _next(context);
                return;
            }
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                attachUserToContext(context, userAuthService, token);

            await _next(context);
        }

        private void attachUserToContext(HttpContext context, IUserAuthService userAuthService, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_options.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _options.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _options.Audience,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "user_id").Value;

                // attach user to context on successful jwt validation
                context.Items["User"] = userAuthService.GetUserById(userId).Result;
            }
            catch(Exception ex)
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
                //var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Oops!!!" };
                //throw new HttpResponseException(msg);
            } 
        }
    }
}
