

using AuthService.Models.DBModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AuthService.Models.ResponseModels
{
    public class UserProfileResponse
    {
        public string? UserUuId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        //public string? UserName { get; set; }
        public string? WalletAddress { get; set; }
        public string? Description { get; set; }
        public string? Mobile { get; set; }
        public string? AvatarUri { get; set; }
        public string? BannerUri { get; set; }
        public string? ClientUuId { get; set; }
        //public bool? IsLoginFirstTime { get; set; }
        public bool? IsOnline { get; set; }
        //public DateTime? ExpiryDate { get; set; }
        //public DateTime? LastLoginDate { get; set; }
        public DateTime? createdDate { get; set; }
        //public DateTime? ModifiedDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
