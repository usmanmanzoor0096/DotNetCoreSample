using AuthService.Models.DBModels;
using AuthService.Models.Models;

namespace AuthService.Data.IRepositories
{
    public interface IUserRepository
    {
        Task<bool> InsertUserTokenAsync(OAuthToken oAuthToken); 
        //Task<int> AddLogOnStats(LogonStat stat);
        //Task<Client> GetUserClientAsync(string userId);
    }
}
