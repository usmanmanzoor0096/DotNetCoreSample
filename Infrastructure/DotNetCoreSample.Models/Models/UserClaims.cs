using System.Security.Claims;

namespace AuthService.Models.Models
{
    public class UserClaims
    {
        public UserClaims()
        {
            TokenClaims = new List<Claim>();
            MemoryClaims = new List<Claim>();
        }
        public List<Claim> TokenClaims { get; set; }
        public List<Claim> MemoryClaims { get; set; }

        public List<Claim> All
        {
            get
            {
                return this.TokenClaims.Concat(this.MemoryClaims).ToList();
            }
        }
    }
}
