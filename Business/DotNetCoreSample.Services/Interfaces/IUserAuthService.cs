using AuthService.Common.Operation;
using AuthService.Models.DBModels;
using AuthService.Models.Models;
using AuthService.Models.RequestModels;
using AuthService.Models.ResponseModels;

namespace AuthService.Services.Interfaces
{
    public interface IUserAuthService
    {
        Task<LoginResult> RegisterUserAsync(UserRegisterRequest userRegisterRequest);
        Task<LoginResult> AuthenticateAsync(TokenRequest tokenRequest);
        Task<LoginResult> RefreshTokenAsync(string appId, string refresh_token);
        Task<ApplicationUser> GetUserById(string userId);
        Task<bool> ForgetPasswordAsync(ForgotPassword forgotPassword);
        Task<UserProfileResponse> UpdateUserProfileAsync(UserProfileUpdateRequest profileUpdateRequest, string loggedInUserId);
        Task<UserProfileResponse> UpdateUserWalletAddressAsync(string loggedInUserId, string walletAddress);
        Task<UserProfileResponse> GetUserPublicProfile(string userName);
    }
}
