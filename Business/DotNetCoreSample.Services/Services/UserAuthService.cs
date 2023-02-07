using AuthService.Common.Extensions;
using AuthService.Common.HttpClient;
using AuthService.Common.Models.ConfigModels;
using AuthService.Common.Operation;
using AuthService.Data.IRepositories;
using AuthService.Models.Config;
using AuthService.Models.DBModels;
using AuthService.Models.Dtos;
using AuthService.Models.Enum;
using AuthService.Models.Models;
using AuthService.Models.RequestModels;
using AuthService.Models.ResponseModels;
using AuthService.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static AuthService.Common.Enums.APIEnums;

namespace AuthService.Business.Services
{
    public class UserAuthService : IUserAuthService
    { 
        private readonly ILogger<UserAuthService> _logger;
        private readonly IOptions<AuthOptions> _options; 
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IEmailService _emailService; 

        public UserAuthService
        ( 
            ILogger<UserAuthService> logger,
            IOptions<AuthOptions> options, 
            UserManager<ApplicationUser> userManager,
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IEmailService emailService
        )
        {
            _logger = logger;
            _options = options;
            ThrowIfInvalidOptions(_options.Value);
            _userManager = userManager;
            _userRepository = userRepository; 
            _refreshTokenRepository = refreshTokenRepository;
            _emailService = emailService; 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userRegisterRequest"></param>
        /// <returns></returns>
        public async Task<LoginResult> RegisterUserAsync(UserRegisterRequest userRegisterRequest)
        {
            var applicationUser = new ApplicationUser
            {
                FirstName = userRegisterRequest.FirstName,
                LastName = userRegisterRequest.LastName,
                Email = userRegisterRequest.UserEmail,
                NormalizedEmail = userRegisterRequest.UserEmail,
                UserName = userRegisterRequest.UserEmail,
                NormalizedUserName = userRegisterRequest.UserEmail,
                ClientId = userRegisterRequest.ClientId,
                ExpiryDate = DateTime.UtcNow.AddYears(50),
                IsActive = true,
                EmailConfirmed = false
            };
            var result = await _userManager.CreateAsync(applicationUser, userRegisterRequest.Password ?? "DotNetCoreSample@123");
            if (result.Succeeded)
            {
                var role = EnumerationExtensions.ToDescription(userRegisterRequest.UserRole);
                var response = await _userManager.AddToRoleAsync(applicationUser, role);
                if (response.Succeeded)
                {
                    return new LoginResult
                    {
                        Clientid = userRegisterRequest.ClientId,
                        ExpiredDate = DateTime.UtcNow.AddMinutes(10),
                        Succeeded = response.Succeeded
                    };
                }
            }
            return new LoginResult
            {
                Error = new CustomeError()
                {
                    Heading = result.Errors.FirstOrDefault().Code,
                    Text = result.Errors.FirstOrDefault().Description
                }
            };
        }

        /// <summary>
        /// AccountService : Service method is responsible to update user basic details, along with the Avatar and banner
        /// </summary>
        /// <param name="profileUpdateRequest"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<UserProfileResponse> UpdateUserProfileAsync(UserProfileUpdateRequest profileUpdateRequest, string loggedInUserId)
        {
            var userInfo = await _userManager.FindByIdAsync(loggedInUserId);
            if (userInfo == null)
                throw new Exception(MessageResource.GetMessage(Common.Enums.APIEnums.APIStatusCodes.UserNotFound));

            userInfo.FirstName = profileUpdateRequest.FirstName;
            userInfo.LastName = profileUpdateRequest.LastName;
            userInfo.Description = profileUpdateRequest.Description;
            userInfo.Avatar = profileUpdateRequest.UserAvatarUri;
            userInfo.BannerUri = profileUpdateRequest.BannerUri;
            //if(!string.IsNullOrEmpty(walletAddress))
            //var fileResponse = await UploadUserProFiles(profileUpdateRequest.userProfileMediaFileRequest);

            var result = await _userManager.UpdateAsync(userInfo);
            if (result.Succeeded)
                return new UserProfileResponse
                {
                    UserUuId = userInfo.Id,
                    ClientUuId = userInfo.ClientId,
                    AvatarUri = userInfo.Avatar,
                    BannerUri = userInfo.BannerUri,
                    Email = userInfo.Email,
                    FirstName = userInfo.FirstName,
                    WalletAddress = userInfo.BloctoWalletAddress,
                    Description = userInfo.Description,
                    LastName = userInfo.LastName,
                    IsActive = userInfo.IsActive,
                    IsOnline = userInfo.IsOnline,
                    Mobile = userInfo.Mobile,
                    createdDate = userInfo.CreateDate
                };
            else
                throw new Exception(MessageResource.GetMessage(Common.Enums.APIEnums.APIStatusCodes.Error));
        }

        /// <summary>
        /// AccountService : Service method to update wallet address
        /// </summary>
        /// <param name="profileUpdateRequest"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<UserProfileResponse> UpdateUserWalletAddressAsync(string loggedInUserId, string walletAddress)
        {
            var userInfo = await _userManager.FindByIdAsync(loggedInUserId);
            if (userInfo == null)
                throw new Exception(MessageResource.GetMessage(Common.Enums.APIEnums.APIStatusCodes.UserNotFound));

            userInfo.BloctoWalletAddress = walletAddress;

            var result = await _userManager.UpdateAsync(userInfo);
            if (result.Succeeded)
                return new UserProfileResponse
                {
                    UserUuId = userInfo.Id,
                    ClientUuId = userInfo.ClientId,
                    AvatarUri = userInfo.Avatar,
                    BannerUri = userInfo.BannerUri,
                    Email = userInfo.Email,
                    FirstName = userInfo.FirstName,
                    WalletAddress = userInfo.BloctoWalletAddress,
                    Description = userInfo.Description,
                    LastName = userInfo.LastName,
                    IsActive = userInfo.IsActive,
                    IsOnline = userInfo.IsOnline,
                    Mobile = userInfo.Mobile,
                    createdDate = userInfo.CreateDate
                };
            else
                throw new Exception(MessageResource.GetMessage(Common.Enums.APIEnums.APIStatusCodes.Error));
        }


        /// <summary>
        /// AccountService : Get User Public Profile Data
        /// </summary>
        /// <param name="profileUpdateRequest"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<UserProfileResponse> GetUserPublicProfile(string userId)
        {
            var userInfo = await _userManager.FindByIdAsync(userId);
            if (userInfo == null)
                throw new Exception(MessageResource.GetMessage(Common.Enums.APIEnums.APIStatusCodes.UserNotFound));

            return new UserProfileResponse
            {
                UserUuId = userInfo.Id,
                ClientUuId = userInfo.ClientId,
                WalletAddress = userInfo.BloctoWalletAddress,
                Description = userInfo.Description,
                AvatarUri = userInfo.Avatar,
                BannerUri = userInfo.BannerUri,
                Email = userInfo.Email,
                FirstName = userInfo.FirstName,
                //LastLoginDate = userInfo.LastLoginDate,
                LastName = userInfo.LastName,
                IsActive = userInfo.IsActive,
                //UserName = userInfo.UserName,
                createdDate = userInfo.CreateDate
            };
        }
         
        /// <summary>
        /// AccountService : Service call for forget password check email from db and send email to reset password.
        /// </summary>
        /// <param name="forgotPassword"></param>
        /// <returns></returns>
        public async Task<bool> ForgetPasswordAsync(ForgotPassword forgotPassword)
        {
            var user = await _userManager.FindByEmailAsync(forgotPassword.Email);
            if (user != null)
            {
                EmailDto email = new EmailDto()
                {
                    EmailAddress = forgotPassword.Email,
                    EmailType = EmailTemplate.ForgotPasswordEmail,
                    SiteUrl = forgotPassword.ClientUri ?? "",
                    User = user
                };
                return await _emailService.SendForgetpasswordEmailAsync(email);
            }
            else
                return false;
        }

        public async Task<bool> RegisterClientEmail(EmailDto emailDto)
        {
            return await _emailService.RegisterClientEmail(emailDto);
        }


        public async Task<LoginResult> AuthenticateAsync(TokenRequest tokenRequest)
        {
            try
            {
                ApplicationUser user = await _userManager.FindByEmailAsync(tokenRequest.Email);
                if (user == null)
                    return new LoginResult { Error = new InvalidUserNamePassword() };
                else if (user.IsActive == false)
                    return new LoginResult { Error = new InActiveUser() };
                else
                {
                    if (!String.IsNullOrEmpty(tokenRequest.Password) && !await _userManager.CheckPasswordAsync(user, tokenRequest.Password))
                    {
                        _logger.LogInformation(1, $"Login failed, invalid password for user '{tokenRequest.Email}'.");
                        return new LoginResult { Error = new InvalidUserNamePassword() };
                    }

                    if ((user.IsDeleted.HasValue && user.IsDeleted.Value) || (user.IsActive.HasValue && !user.IsActive.Value))
                    {
                        _logger.LogInformation(1, $"Login failed, user '{tokenRequest.Email}' is not active.");
                        return new LoginResult { Error = new InActiveUser() };
                    }

                    //check if user is expired
                    if (user.ExpiryDate.HasValue && user.ExpiryDate.Value.Date < DateTime.Today)
                    {
                        if (await _userManager.IsInRoleAsync(user, "Trial User"))
                        {
                            _logger.LogInformation(1, $"Login failed, this is a trial user '{tokenRequest.Email}' and has been expired.");
                            return new LoginResult { Error = new TrialUserExpired() };
                        }
                        else
                        {
                            _logger.LogInformation(1, $"Login failed, user '{tokenRequest.Email}' has been expired.");
                            return new LoginResult { Error = new UserExpired() };
                        }

                    }

                    _logger.LogInformation(1, "User logged in.");

                    string refreshToken = Guid.NewGuid().ToString(); //.Replace("-", "");
                    //bool refreshTokenSaved = false;
                    //if (tokenRequest.RememberMe)
                    //{
                        var rToken = new RefreshToken
                        {
                            Id = Guid.NewGuid(),
                            UserId = user.Id.ToString(),
                            Token = refreshToken,
                            Expired = false,
                            Email = tokenRequest.Email
                        };
                        //store the refresh_token 
                        await _refreshTokenRepository.AddToken(rToken);
                    //}

                    var authToken = await GenerateToken(user, refreshToken, user.ClientId, null, tokenRequest.Email, tokenRequest.Password);
                    if (authToken == null)
                    {
                        _logger.LogInformation(1, "Cannot generate token.");
                        return new LoginResult { Error = new FailedAccessToken() };
                    }
                    var userRole = await _userManager.GetRolesAsync(user);

                    return new LoginResult
                    {
                        Token = authToken,
                        Succeeded = true,
                        Role = string.Join(",", userRole.ToArray()),
                        Email = user.Email,
                        Clientid = user.ClientId,
                        ExpiredDate = user.ExpiryDate.HasValue ? user.ExpiryDate.Value : DateTime.Now.AddMinutes(_options.Value.Expiration)
                    };
                }

            }
            catch (Exception ex)
            {
                return new LoginResult { Error = new SystemGeneratedError() { Text = "Error occurred: " + ex.Message } };
            }

        }

        public async Task<ApplicationUser> GetUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user;
        }
        public async Task<LoginResult> RefreshTokenAsync(string userUuId, string refresh_token)
        {
            var refreshToken = await _refreshTokenRepository.GetToken(refresh_token, userUuId);

            if (refreshToken == null)
            {
                _logger.LogInformation(1, "Cannot refresh token.");
                return new LoginResult { Error = new FailedRefreshToken() };
            }

            if (refreshToken.Expired)
            {
                _logger.LogInformation(1, "Refresh token is expired.");
                return new LoginResult { Error = new FailedRefreshToken() };
            }

            refreshToken.Expired = true;

            //expire the old refresh_token and add a new refresh_token
            var updateFlag = await _refreshTokenRepository.ExpireToken(refreshToken.UserId, refreshToken.Token);

            refresh_token = Guid.NewGuid().ToString().Replace("-", "");
            var addFlag = await _refreshTokenRepository.AddToken(new RefreshToken
            {
                UserId = userUuId,
                Token = refresh_token,
                Id = Guid.NewGuid(),
                Expired = false,
                Email = refreshToken.Email
            });

            if (updateFlag && addFlag)
            {
                ApplicationUser user = await _userManager.FindByEmailAsync(refreshToken.Email);
                if (user != null && user.IsActive == false)
                    return new LoginResult { Error = new InActiveUser() };
                else
                {
                    var authToken = await GenerateToken(user, refresh_token, userUuId);
                    if (authToken == null)
                    {
                        _logger.LogInformation(1, "Cannot generate new token using refresh token.");
                        return new LoginResult { Error = new FailedRefreshToken() };
                    }
                    return new LoginResult { Token = authToken, Succeeded = true };
                }

            }
            else
            {
                _logger.LogInformation(1, "Cannot generate new token using refresh token.");
                return new LoginResult { Error = new FailedRefreshToken() };
            }
        }


        private async Task<OAuthToken> GenerateToken(ApplicationUser user,string refresh_token,string client_Id,int? orginalUserId = null,string userName = null, string password = null)
        {
            try
            {
                DateTime now = DateTime.UtcNow;
                string sessionId = Guid.NewGuid().ToString();
                var userClaims = await GetUserClaimsAsync(now, sessionId, user, client_Id, orginalUserId, userName, password);

                var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.Value.Secret));
                var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                // Create the JWT and write it to a string
                var jwt = new JwtSecurityToken(
                    issuer: _options.Value.Issuer,
                    audience: _options.Value.Audience,
                    claims: userClaims.All,
                    notBefore: now,
                    expires: now.AddMinutes(_options.Value.Expiration),
                    signingCredentials: signingCredentials);

                var handler = new JwtSecurityTokenHandler();
                var encodedJwt = handler.WriteToken(jwt);

                var token = new OAuthToken
                {
                    user_id = user.Id.ToString(),
                    access_token = encodedJwt,
                    expires_in = Convert.ToInt32(_options.Value.Expiration * 60), // In Seconds
                    refresh_token = refresh_token,
                    Session_id = sessionId
                };

                await _userRepository.InsertUserTokenAsync(token);
                 
                return token;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task<UserClaims> GetUserClaimsAsync(DateTime createdOn, string sessionId, ApplicationUser user, string client_Id, int? orginalUserId = null, string userName = null,
            string password = null)
        {
            try
            {
                UserClaims userClaims = new UserClaims();
                //get user claims here
                IEnumerable<Claim> claims = await _userManager.GetClaimsAsync(user);

                //exclude following claims
                userClaims.MemoryClaims = claims.Where(c =>
                    c.Type != CustomClaims.EmailSignature &&
                    c.Type != CustomClaims.EmailPassword &&
                    c.Type != CustomClaims.UserId &&
                    c.Type != CustomClaims.ClientId &&
                    c.Type != CustomClaims.ClientName &&
                    c.Type != CustomClaims.ClientAddress &&
                    c.Type != CustomClaims.Mobile &&
                    c.Type != CustomClaims.UserRole &&
                    c.Type != JwtRegisteredClaimNames.GivenName &&
                    c.Type != JwtRegisteredClaimNames.FamilyName &&
                    c.Type != JwtRegisteredClaimNames.Email).ToList();

                userClaims.TokenClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, await _options.Value.NonceGenerator()));
                userClaims.TokenClaims.Add(new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(createdOn).ToUniversalTime().ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64));
                userClaims.TokenClaims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.UserName));
                userClaims.TokenClaims.Add(new Claim(CustomClaims.UserId, user.Id.ToString()));

                userClaims.MemoryClaims.Add(new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName == null ? "" : user.FirstName));
                userClaims.MemoryClaims.Add(new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName == null ? "" : user.LastName));
                userClaims.MemoryClaims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
                userClaims.MemoryClaims.Add(new Claim(CustomClaims.ClientId, client_Id ?? ""));
                userClaims.MemoryClaims.Add(new Claim(CustomClaims.Avatar, string.IsNullOrEmpty(user.Avatar) ? "" : user.Avatar));
                userClaims.MemoryClaims.Add(new Claim(CustomClaims.PasswordChangeRequired, user.PasswordChangeRequired.ToString()));

                // add user roles to claims
                var userRoles = await _userManager.GetRolesAsync(user);
                if (userRoles != null && userRoles.Count > 0)
                {
                    userClaims.MemoryClaims.AddRange(userRoles.Select(r => { return new Claim(ClaimTypes.Role, r); }).ToList());
                }
                userClaims.MemoryClaims.Add(new Claim(CustomClaims.UserRole, String.Join(" ", userRoles.ToList()))); // userRoles.Select(r => { return new Claim(ClaimTypes.Role, r); }).ToList())));

                //in case of login as other user, then keep the user id who inititate login as request
                if (orginalUserId.HasValue)
                {
                    userClaims.MemoryClaims.Add(new Claim(CustomClaims.OriginalUserId, orginalUserId.Value.ToString()));
                }

                userClaims.MemoryClaims.Add(new Claim(CustomClaims.LoginAsUser, orginalUserId.HasValue ? "True" : "False"));

                var idletime = userClaims.MemoryClaims.FirstOrDefault(x => x.Type == CustomClaims.Idle_Time);
                if (idletime == null)
                {
                    userClaims.MemoryClaims.Add(new Claim(CustomClaims.Idle_Time, _options.Value.Expiration.ToString()));
                }

                userClaims.MemoryClaims.Add(new Claim(CustomClaims.SessionId, sessionId));

                return userClaims;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private static void ThrowIfInvalidOptions(AuthOptions options)
        {
            if (string.IsNullOrEmpty(options.Secret))
            {
                throw new ArgumentNullException(nameof(AuthOptions.Secret));
            }

            if (string.IsNullOrEmpty(options.Issuer))
            {
                throw new ArgumentNullException(nameof(AuthOptions.Issuer));
            }

            if (string.IsNullOrEmpty(options.Audience))
            {
                throw new ArgumentNullException(nameof(AuthOptions.Audience));
            }

            if (options.Expiration <= 0)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(AuthOptions.Expiration));
            }

            if (options.NonceGenerator == null)
            {
                throw new ArgumentNullException(nameof(AuthOptions.NonceGenerator));
            }
        }


    }
}
