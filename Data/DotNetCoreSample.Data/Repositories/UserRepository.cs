using AuthService.Data.Context;
using AuthService.Data.IRepositories;
using AuthService.Models.DBModels; 

namespace AuthService.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlDBContext _sqlDBContext;
        public UserRepository(SqlDBContext mySqlDBContext)
        {
            _sqlDBContext = mySqlDBContext;
        }

      
        public async Task<bool> InsertUserTokenAsync(OAuthToken oAuthToken)
        {
            try
            {
                var result = await _sqlDBContext.OAuthTokens.AddAsync(oAuthToken);
                if (result != null)
                    return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        } 

        //public async Task<string> GetEmailtemplate(string templateName)
        //{
        //    using (var context = new MySqlContext())
        //    {
        //        return await context.EmailTemplates.Where(x => x.TemplateName == templateName).Select(x => x.TemplateValue).FirstOrDefaultAsync();
        //    }
        //}

        //public async Task<int> AddLogOnStats(LogonStat stat)
        //{
        //    using (_sqlDBContext)
        //    {
        //        DynamicParameters param = new DynamicParameters();
        //        param.Add("@IP", stat.IP);
        //        param.Add("@UserId", stat.UserId);
        //        param.Add("@Browser", stat.Browser);
        //        param.Add("@Device", stat.Device);
        //        param.Add("@HostName", stat.HostName);
        //        param.Add("@SessionId", stat.SessionId);
        //        param.Add("@LoggedInAsUser", stat.LoggedInAsUser);
        //        param.Add("@LoggedOut", false);

        //        if (stat.OriginalUserId.HasValue && stat.OriginalUserId.Value > 0)
        //            param.Add("@OriginalUserId", stat.OriginalUserId.Value);

        //        var result = await connection.ExecuteAsync("InsertLogOnStats", param, commandType: CommandType.StoredProcedure);

        //        return result > 0 ? stat.UserId : 0;
        //    }
        //}

        //public Task<Client> GetUserClientAsync(string userId)
        //{
        //    return null;
        //}
    }
}
