
using AuthService.Models.Dtos;

namespace AuthService.Models.Models
{
    public class LoggedInUser
    {
        public LoggedInUser()
        {
            Permissions = new List<ClaimDto>();
        }
        public int UserId { get; set; }
        public bool Active { get; set; }
        public DateTime ExpiryDate { get; set; }
        public List<ClaimDto> Permissions { get; set; }
    }
}
