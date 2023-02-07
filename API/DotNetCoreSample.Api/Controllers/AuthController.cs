using AuthService.Common.Extensions;
using AuthService.Common.Models.ConfigModels;
using AuthService.Communication.SQService;
using AuthService.Models.AWS.DTOs;
using AuthService.Models.DBModels;
using AuthService.Models.Enum;
using AuthService.Models.Models;
using AuthService.Models.RequestModels;
using AuthService.Models.ResponseModel;
using AuthService.Services.Interfaces;
using Google.Apis.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Cms;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using static AuthService.Common.Enums.APIEnums;

namespace AuthService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    [Authorize]
    public class AuthController : SecureController
    {
        private readonly IUserAuthService _userAuthService;
        private readonly IEmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AuthController> _logger;
        private readonly ISQSEmailService _sQSEmailService;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly AppConfig _appConfig;

        public AuthController(IUserAuthService userAuthService, UserManager<ApplicationUser> userManager, IEmailService emailService, ILogger<AuthController> logger, ISQSEmailService sQSEmailService, RoleManager<IdentityRole> roleManager, AppConfig appConfig)
        {
            _userAuthService = userAuthService;
            _userManager = userManager;
            _emailService = emailService;
            _logger = logger;
            _sQSEmailService = sQSEmailService;
            this.roleManager = roleManager;
            _appConfig = appConfig;
        }

        /// <summary>
        /// LogIn API : An API end point to validate user once user provide login details.
        /// </summary>
        /// <param name="tokenRequest">It will contains user information to validate user account.</param>
        /// <returns>A newly created user data with JWT token</returns>  
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Auth/login
        ///     {
        ///        GrantType: "Password, RefreshToken",  
        ///        Email: "test@test.com",
        ///        Password: "****",
        ///        UserId: "GUID", 
        ///        RememberMe:  "false"
        ///     }
        /// </remarks>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">response status code indicates that the server cannot or will not process the request due to something that is perceived to be a client error (for example, malformed request syntax, invalid request message framing, or deceptive request routing)</response>
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [Produces("application/json")]
        [AllowAnonymous, HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest tokenRequest)
        {
            _logger.LogInformation("login sync....");
            if (tokenRequest.GrantType != GrandType.Password)
            {
                _logger.LogError(new EventId(1000), "Invalid grant type.");
                return BadRequest(new LoginResult { Error = new InvalidGrantType() });
            }

            var result = await _userAuthService.AuthenticateAsync(new TokenRequest
            {
                GrantType = EnumerationExtensions.ToDescription(tokenRequest.GrantType).ToString().ToLower(),
                //UserId = tokenRequest.UserId,
                Email = tokenRequest.Email,
                Password = tokenRequest.Password,
                RememberMe = tokenRequest.RememberMe
            });

            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }


        /// <summary>
        /// New Access Token: This API end point will responsible to generate new access token based on the refresh token. Whenever token expires the system will expect the refresh token from client side, Once system validate the refresh token provided by client the system will generate new access token.
        /// </summary>
        /// <param name="tokenRequest">parameters will contain token data.</param>
        /// <returns>A newly created user data with JWT token</returns>  
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Auth/get_new_access_token
        ///     {
        ///        GrantType: "Password, RefreshToken",  
        ///        RefreshToken: "GUID",
        ///        UserId: "GUID"  
        ///     }
        /// </remarks>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">response status code indicates that the server cannot or will not process the request due to something that is perceived to be a client error (for example, malformed request syntax, invalid request message framing, or deceptive request routing)</response>
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [Produces("application/json")]
        [AllowAnonymous, HttpPost("get_new_access_token")]
        public async Task<IActionResult> LoginWithRefreshTokenAsync(LoginRequestWithRefreshToken tokenRequest)
        {
            _logger.LogInformation("login async....");
            if (tokenRequest.GrantType != GrandType.RefreshToken)
            {
                _logger.LogError(new EventId(1000), "Invalid grant type.");
                return BadRequest(new LoginResult { Error = new InvalidGrantType() });
            }
            var result = await _userAuthService.RefreshTokenAsync(tokenRequest.UserUuId, tokenRequest.RefreshToken);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }


        /// <summary>
        /// Register New Client User: This API end point will responsible to register new user based on the given detail.
        /// </summary>
        /// <param name="userRegister">This will contains user data to create new user account</param>
        /// <returns>A newly created user data with JWT token</returns>  
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Auth/register_client
        ///     {
        ///         FirstName: "Test",
        ///         LastName:  "User",
        ///         UserEmail: "test@test.com",
        ///         UserId:  "GUID", 
        ///     }
        /// </remarks>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">response status code indicates that the server cannot or will not process the request due to something that is perceived to be a client error (for example, malformed request syntax, invalid request message framing, or deceptive request routing)</response>
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [Produces("application/json")]
        [AllowAnonymous, HttpPost("register_client")]
        public async Task<IActionResult> RegisterClient([FromBody] ClientRegisterRequest userRegister)
        {
            if (ModelState.IsValid)
            {
                var result = await _userAuthService.RegisterUserAsync(new UserRegisterRequest
                {
                    ClientId = userRegister.ClientId,
                    FirstName = userRegister.FirstName,
                    LastName = userRegister.LastName,
                    UserEmail = userRegister.UserEmail,
                    UserRole = Roles.CLIENT
                });
                if (result.Succeeded)
                {
                    //Send UserEmail to client to set his password.
                    var user = await _userManager.FindByEmailAsync(userRegister.UserEmail);
                    if (user != null)
                    {
                        await _emailService.RegisterClientEmail(new Models.Dtos.EmailDto
                        {
                            EmailAddress = user.Email,
                            User = user
                        });
                    }
                    return GenericApiResponse(result, MessageResource.GetMessage(APIStatusCodes.Success), System.Net.HttpStatusCode.OK);
                }
                return GenericApiResponse(result, result.Error.Text, System.Net.HttpStatusCode.NotFound);
            }
            return GenericApiResponse(default(IActionResult), MessageResource.GetMessage(APIStatusCodes.Error), System.Net.HttpStatusCode.Forbidden);
        }

        [HttpPost("register-admin")]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] ClientRegisterRequest model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.UserEmail);
            if (userExists != null)
                return GenericApiResponse(default(IActionResult), MessageResource.GetMessage(APIStatusCodes.UserAlreadyRegistered), System.Net.HttpStatusCode.NotFound);

            ApplicationUser user = new ApplicationUser()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.UserEmail,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserEmail
            };
            var result = await _userManager.CreateAsync(user, "DotNetCoreSample@123");
            if (!result.Succeeded)
                return GenericApiResponse(default(IActionResult), MessageResource.GetMessage(APIStatusCodes.Error), System.Net.HttpStatusCode.InternalServerError);


            if (!await roleManager.RoleExistsAsync(EnumerationExtensions.ToDescription(Roles.ADMIN)))
                await roleManager.CreateAsync(new IdentityRole(EnumerationExtensions.ToDescription(Roles.ADMIN)));
            if (!await roleManager.RoleExistsAsync(EnumerationExtensions.ToDescription(Roles.USER)))
                await roleManager.CreateAsync(new IdentityRole(EnumerationExtensions.ToDescription(Roles.USER)));

            if (await roleManager.RoleExistsAsync(EnumerationExtensions.ToDescription(Roles.ADMIN)))
            {
                await _userManager.AddToRoleAsync(user, EnumerationExtensions.ToDescription(Roles.ADMIN));
            }

            return GenericApiResponse(default(IActionResult), MessageResource.GetMessage(APIStatusCodes.Success), System.Net.HttpStatusCode.OK);

        }


        /// <summary>
        /// Register New User: This API end point will responsible to register new user based on the given detail.
        /// </summary>
        /// <param name="userRegister">This will contains user data to create new user account</param>
        /// <returns>A newly created user data with JWT token</returns>  
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Auth/register_user
        ///     {
        ///         FirstName: "Test",
        ///         LastName:  "User",
        ///         UserEmail: "test@test.com",
        ///         Password: "****",
        ///         ConfirmPassword: "****",
        ///         UserId:  "GUID",
        ///         UserRole:  "User",
        ///         RememberMe: false
        ///     }
        /// </remarks>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">response status code indicates that the server cannot or will not process the request due to something that is perceived to be a client error (for example, malformed request syntax, invalid request message framing, or deceptive request routing)</response>
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [Produces("application/json")]
        [AllowAnonymous, HttpPost("register_user")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegisterRequest userRegister)
        {
            if (ModelState.IsValid)
            {
                var result = await _userAuthService.RegisterUserAsync(userRegister);
                if (result.Succeeded)
                {
                    //Send Email to client to set his password.
                    var user = await _userManager.FindByEmailAsync(userRegister.UserEmail);
                    if (user != null)
                    {
                        var token = await _userAuthService.AuthenticateAsync(new TokenRequest
                        {
                            GrantType = EnumerationExtensions.ToDescription(GrandType.Password).ToString().ToLower(),
                            ClientId = user.ClientId,
                            Email = user.Email,
                            UserUuId = user.Id,
                            Password = userRegister.Password,
                            RememberMe = userRegister.RememberMe
                        });

                        if (!token.Succeeded)
                            return GenericApiResponse(token, MessageResource.GetMessage(APIStatusCodes.Error), System.Net.HttpStatusCode.NotFound);


                        return GenericApiResponse(token, MessageResource.GetMessage(APIStatusCodes.Success), System.Net.HttpStatusCode.OK);

                    }
                    return GenericApiResponse(result, MessageResource.GetMessage(APIStatusCodes.UserRegistrationError), System.Net.HttpStatusCode.ExpectationFailed);
                }
                return GenericApiResponse(result, result.Error.Text, System.Net.HttpStatusCode.NotFound);
            }
            return GenericApiResponse(default(IActionResult), MessageResource.GetMessage(APIStatusCodes.Error), System.Net.HttpStatusCode.Forbidden);

        }


        /// <summary>
        /// Find User: This end point will check user is already register with us or not!
        /// </summary>
        /// <param name="userRequestModel">This will need user email in a parameter</param>
        /// <returns>A newly created user data</returns>  
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Auth/register_user
        ///     {
        ///         UserEmail: "test@test.com" 
        ///     }
        /// </remarks>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">response status code indicates that the server cannot or will not process the request due to something that is perceived to be a client error (for example, malformed request syntax, invalid request message framing, or deceptive request routing)</response>
        [ProducesResponseType(typeof(ApplicationUser), StatusCodes.Status200OK)]
        [Produces("application/json")]
        [HttpPost("find_user_by_email")]
        public async Task<IActionResult> FindUserByEmail([FromBody] UserRequestModel userRequestModel)
        {
            //Send UserEmail to client to set his password.
            var user = await _userManager.FindByEmailAsync(userRequestModel.Email);
            if (user != null)
                return GenericApiResponse(user, MessageResource.GetMessage(APIStatusCodes.Success), System.Net.HttpStatusCode.OK);

            return GenericApiResponse(user, MessageResource.GetMessage(APIStatusCodes.UserNotFound), System.Net.HttpStatusCode.NotFound);
        }


        /// <summary>
        /// Set Password: This API end point will update user password based on the given detail. This api will dependent on the forgot password flow, when user try to update the password the system will require the Code which be generate on Client registration/Forgot password. 
        /// </summary> 
        /// <returns>The system will return Success Or Failure response.</returns>  
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /Auth/set_password
        ///     {
        ///         UserId: "GUID",
        ///         Code: "Hexa decimal code",
        ///         Email : "test@test.com",
        ///         Password: "*****"
        ///     }
        /// </remarks>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">response status code indicates that the server cannot or will not process the request due to something that is perceived to be a client error (for example, malformed request syntax, invalid request message framing, or deceptive request routing)</response>
        [AllowAnonymous, HttpPut("reset_password")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetPassword([FromBody] SetPassword resetPassword)
        {
            var user = await _userManager.FindByIdAsync(resetPassword.UserId);
            if (user != null)
            {
                string code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetPassword.Code));
                IdentityResult result = await _userManager.ResetPasswordAsync(user, code, resetPassword.Password);

                if (result.Succeeded)
                {
                    List<string> recipients = new List<string>();
                    recipients.Add(user.Email);
                    SqsMessageModel sqsMessageModel = new SqsMessageModel
                    {
                        Subject = "Password set Successfully",
                        Recipients = recipients,
                        Link = String.IsNullOrEmpty(resetPassword.ClientUri) ? _appConfig.BaseUri + "/login" : resetPassword.ClientUri,
                        TemplateType = EmailTemplate.SetPasswordTemplate.ToString()
                    };
                    await _sQSEmailService.SendEmailViaSQS(sqsMessageModel);

                    return GenericApiResponse(result, MessageResource.GetMessage(APIStatusCodes.Success), System.Net.HttpStatusCode.OK);
                }
                else
                    return GenericApiResponse(result, result.Errors.FirstOrDefault().Description, System.Net.HttpStatusCode.Forbidden);
            }
            return GenericApiResponse(user, MessageResource.GetMessage(APIStatusCodes.Error), System.Net.HttpStatusCode.ExpectationFailed);
        }

        /// <summary>
        /// Reset Password: This API end point will update user password based on the given detail. This api will dependent on the forgot password flow, when user try to update the password the system will require the Code which be generate on Client registration/Forgot password. 
        /// </summary> 
        /// <returns>The system will return Success Or Failure response.</returns>  
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /Auth/change_password
        ///     {
        ///      "oldPassword": "string",
        ///      "password": "string",
        ///      "confirmPassword": "string"
        ///     }
        /// </remarks>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">response status code indicates that the server cannot or will not process the request due to something that is perceived to be a client error (for example, malformed request syntax, invalid request message framing, or deceptive request routing)</response>
        [Authorize, HttpPut("change_password")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangePassword([FromBody] ResetPassword resetPassword)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            ApplicationUser user = new ApplicationUser();
            user = (ApplicationUser)this.Request.HttpContext.Items["User"];
            if (user != null)
            {
                IdentityResult result = await _userManager.ChangePasswordAsync(user, resetPassword.OldPassword, resetPassword.Password);
                if (result.Succeeded)
                {
                    List<string> recipients = new List<string>();
                    recipients.Add(user.Email);
                    SqsMessageModel sqsMessageModel = new SqsMessageModel
                    {
                        Subject = "Your Password Updated Successfully",
                        Recipients = recipients,
                        Link = String.IsNullOrEmpty(resetPassword.ClientUri) ? _appConfig.BaseUri + "/login" : resetPassword.ClientUri,
                        TemplateType = EmailTemplate.SetPasswordTemplate.ToString()
                    };
                    await _sQSEmailService.SendEmailViaSQS(sqsMessageModel);

                    return GenericApiResponse(result, MessageResource.GetMessage(APIStatusCodes.Success), System.Net.HttpStatusCode.OK);
                }
                else
                    return GenericApiResponse(result.Errors.FirstOrDefault(), result.Errors.FirstOrDefault().Description, System.Net.HttpStatusCode.NotFound);
            }
            return GenericApiResponse(user, MessageResource.GetMessage(APIStatusCodes.Error), System.Net.HttpStatusCode.ExpectationFailed);
        }


        /// <summary>
        /// This API end point will generate an email on a given input email by user, Which let user to update the password.
        /// </summary> 
        /// <returns>The system will return Success Or Failure response.</returns>  
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /Auth/forgot_password
        ///     {
        ///         Email : "test@test.com"
        ///         ClientUri: "http://app.com"
        ///     }
        /// </remarks>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">response status code indicates that the server cannot or will not process the request due to something that is perceived to be a client error (for example, malformed request syntax, invalid request message framing, or deceptive request routing)</response>
        [AllowAnonymous, HttpPost("forgot_password")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPassword forgotPassword)
        {
            forgotPassword.ClientUri = String.IsNullOrEmpty(forgotPassword.ClientUri) ? _appConfig.BaseUri +"/set-password" : forgotPassword.ClientUri;
            var result = await _userAuthService.ForgetPasswordAsync(forgotPassword);
            if (result)
                return GenericApiResponse(result, MessageResource.GetMessage(APIStatusCodes.Success), System.Net.HttpStatusCode.OK);
            else
                return GenericApiResponse(result, MessageResource.GetMessage(APIStatusCodes.Error), System.Net.HttpStatusCode.NotFound);
        }

        /// <summary>
        /// This API end point will validate token! this validation is based on their expiry and other 
        /// </summary> 
        /// <returns>A newly created user data with JWT token</returns>  
        /// <remarks>
        /// Sample request:
        ///     POST /Auth/"validate_token
        /// </remarks>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">response status code indicates that the server cannot or will not process the request due to something that is perceived to be a client error (for example, malformed request syntax, invalid request message framing, or deceptive request routing)</response>
        [HttpPost("validate_token")]
        [Authorize(Roles = "Super Admin,Admin, User, Client")]
        public IActionResult ValidateToken()
        {
            return Ok(new { Success = true, user = this.Request.HttpContext.Items["User"] });
        }

        /// <summary>
        /// This API end point will responsible to active/inactive User by administrator only
        /// Note: This end point will be dependent on the client! whenever Administrator will inactive any client we have to inactive the user as well.
        /// This end point will be used for the client user as well in future!!!
        /// </summary> 
        /// <param name="isActive">This will need boolean value in a parameter</param>
        /// <param name="userId">This will contains user id</param>
        /// <returns>A newly created user data</returns>  
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Auth/register_user
        ///     {
        ///         UserId: "GUID",
        ///         isActive: "True, False" 
        ///     }
        /// </remarks>
        /// <response code="201">Returns success response</response>
        /// <response code="400">response status code indicates that the server cannot or will not process the request due to something that is perceived to be a client error (for example, malformed request syntax, invalid request message framing, or deceptive request routing)</response>
        [ProducesResponseType(typeof(IdentityResult), StatusCodes.Status200OK)]
        //[Authorize(Roles = Role.ADMIN )]
        [HttpPost("active_inactive_user")]
        public async Task<IActionResult> ActiveInactiveUser(bool isActive, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return GenericApiResponse("", "User Id is required.", System.Net.HttpStatusCode.Forbidden);
            else
            {
                var user = await _userManager.FindByIdAsync(userId);
                user.IsActive = isActive;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                    return GenericApiResponse(result, MessageResource.GetMessage(APIStatusCodes.Success), System.Net.HttpStatusCode.OK);
                else
                    return GenericApiResponse(result, MessageResource.GetMessage(APIStatusCodes.Error), System.Net.HttpStatusCode.ExpectationFailed);
            }
        }
    }
}
