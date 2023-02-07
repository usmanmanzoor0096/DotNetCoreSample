using AuthService.Common.Models.DBModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace AuthService.Models.DBModels
{
    public class OAuthToken : BaseEntity
    { 
        [StringLength(1000)]
        [Column(TypeName = "nvarchar(1000)")]
        public string? Token { get; set; }
        [StringLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string? user_id { get; set; }
        [StringLength(1000)]
        [Column(TypeName = "nvarchar(1000)")]
        public string? access_token { get; set; }
        public int expires_in { get; set; }
        [StringLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string? refresh_token { get; set; }
        [StringLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string? Session_id { get; set; }
    }
}
