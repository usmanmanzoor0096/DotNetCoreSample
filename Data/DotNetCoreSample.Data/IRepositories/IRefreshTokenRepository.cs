
using AuthService.Models.DBModels;
using AuthService.Models.Models;

namespace AuthService.Data.IRepositories
{
    public interface IRefreshTokenRepository
    {
        Task<bool> AddToken(RefreshToken token);

        Task<bool> ExpireToken(string id, string token);

        Task<RefreshToken> GetToken(string token, string clientId);
    }
}
