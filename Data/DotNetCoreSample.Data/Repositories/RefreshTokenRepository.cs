using AuthService.Data.Context;
using AuthService.Data.IRepositories;
using AuthService.Models.DBModels; 
using Microsoft.EntityFrameworkCore; 

namespace AuthService.Data.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly SqlDBContext _sqlDBContext;
        public RefreshTokenRepository(SqlDBContext mySqlDBContext)
        {
            _sqlDBContext = mySqlDBContext;
        }

        public async Task<bool> AddToken(RefreshToken token)
        {
            try
            {
                token.IsActive = true;
                token.IsDeleted = false;
                var result = await _sqlDBContext.RefreshToken.AddAsync(token);
                await _sqlDBContext.SaveChangesAsync();
                if (result != null)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;

        }

        public async Task<bool> ExpireToken(string UserId, string token)
        {
            try
            {
                RefreshToken refreshToken = await _sqlDBContext.RefreshToken.Where(x => x.UserId == UserId && x.Token == token).FirstOrDefaultAsync();
                refreshToken.Expired = true;
                var result = _sqlDBContext.RefreshToken.Update(refreshToken);
                await _sqlDBContext.SaveChangesAsync();
                return result != null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        public async Task<RefreshToken> GetToken(string token, string userUuId)
        {
            return await _sqlDBContext.RefreshToken.Where(x => x.Token == token && x.UserId == userUuId).FirstOrDefaultAsync();
        }
    }
}
