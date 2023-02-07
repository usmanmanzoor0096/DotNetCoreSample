using Amazon.Auth.AccessControlPolicy;
using AuthService.Common.Extensions;
using AuthService.Communication.SQService;
using AuthService.Controllers;
using AuthService.Models.DBModels;
using AuthService.Models.Enum;
using AuthService.Models.RequestModels; 
using AuthService.Models.ResponseModels;
using AuthService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using static AuthService.Common.Enums.APIEnums;

namespace AuthService.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : SecureController
    {
        private readonly IUserAuthService _userAuthService;
        private readonly IEmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AuthController> _logger;
        private readonly ISQSEmailService _sQSEmailService;

        public AccountController(IUserAuthService userAuthService, UserManager<ApplicationUser> userManager, IEmailService emailService, ILogger<AuthController> logger, ISQSEmailService sQSEmailService)
        {
            _userAuthService = userAuthService;
            _userManager = userManager;
            _emailService = emailService;
            _logger = logger;
            _sQSEmailService = sQSEmailService;
        }

        /// <summary>
        /// Update user profile data: This API end point will responsible for updating user profile data.
        /// </summary>
        /// <param name="profileUpdateRequest">This will contains user data to create new user account</param>
        /// <returns>A newly created user data with JWT token</returns>  
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Auth/update_user_profile
        ///     {
        ///         FirstName: "Test",
        ///         LastName:  "User",
        ///         Description: "Detail",
        ///         AvatarUri = "Image Url",
        ///         BannerUri = "Image Url"
        ///     }
        /// </remarks>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">response status code indicates that the server cannot or will not process the request due to something that is perceived to be a client error (for example, malformed request syntax, invalid request message framing, or deceptive request routing)</response>
        [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
        [Produces("application/json")]
        //[Authorize(Roles = "Administrator")]
        [HttpPost("update_user_profile")]
        public async Task<IActionResult> UpdateUserInformation([FromForm] UserProfileUpdateRequest profileUpdateRequest)
        {
            var user = (ApplicationUser)this.Request.HttpContext.Items["User"];
            if (user == null)
                return GenericApiResponse(default(IActionResult), "Unauthorized", System.Net.HttpStatusCode.Unauthorized);

            var result = await _userAuthService.UpdateUserProfileAsync(profileUpdateRequest, user.Id);
             if(result != null)
                return GenericApiResponse(result, MessageResource.GetMessage(APIStatusCodes.Success), System.Net.HttpStatusCode.OK);
            
            return GenericApiResponse(result, MessageResource.GetMessage(APIStatusCodes.Error), System.Net.HttpStatusCode.InternalServerError);
        }

        /// <summary>
        /// Update User Wallet: This end point will update user wallet address.
        /// </summary> 
        /// <returns>A newly created user data</returns>  
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Auth/update-user-wallet-address
        ///     {
        ///         walletAddress: "wallet address" 
        ///     }
        /// </remarks>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">response status code indicates that the server cannot or will not process the request due to something that is perceived to be a client error (for example, malformed request syntax, invalid request message framing, or deceptive request routing)</response>
        [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
        [Produces("application/json")]
        [HttpPost("update-user-wallet-address")]
        public async Task<IActionResult> UpdateUserWallet(string walletAddress)
        {
            var user = (ApplicationUser)this.Request.HttpContext.Items["User"];
            if (user == null)
                return GenericApiResponse(default(IActionResult), "Unauthorized", System.Net.HttpStatusCode.Unauthorized);

            var result = await _userAuthService.UpdateUserWalletAddressAsync(user.Id, walletAddress);
            if (result != null)
                return GenericApiResponse(result, MessageResource.GetMessage(APIStatusCodes.Success), System.Net.HttpStatusCode.OK);

            return GenericApiResponse(result, MessageResource.GetMessage(APIStatusCodes.Error), System.Net.HttpStatusCode.InternalServerError);
        }


        /// <summary>
        /// Get User Data: This API will provide user public profile data.
        /// </summary>
        /// <param name="userId"></param> 
        /// <returns>A newly created user data with JWT token</returns>  
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Auth/get-user-public-profile
        ///     {
        ///         userId: "User Id"
        ///     }
        /// </remarks>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">response status code indicates that the server cannot or will not process the request due to something that is perceived to be a client error (for example, malformed request syntax, invalid request message framing, or deceptive request routing)</response>
        [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
        [Produces("application/json")]
        [AllowAnonymous, HttpPost("get-user-public-profile")]
        public async Task<IActionResult> GetUserPublicProfile(string userId)
        {
            var result = await _userAuthService.GetUserPublicProfile(userId);
            if (result != null)
                return GenericApiResponse(result, MessageResource.GetMessage(APIStatusCodes.Success), System.Net.HttpStatusCode.OK);

            return GenericApiResponse(result, MessageResource.GetMessage(APIStatusCodes.Error), System.Net.HttpStatusCode.InternalServerError);
        }

    }
}
