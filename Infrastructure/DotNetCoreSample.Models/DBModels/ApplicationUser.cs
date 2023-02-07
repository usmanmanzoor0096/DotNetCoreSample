using AuthService.Common.Models.DBModels;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Models.DBModels
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string? FirstName { get; set; }

        [StringLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string? LastName { get; set; }

        [StringLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string? BloctoWalletAddress { get; set; }

        [StringLength(1000)]
        [Column(TypeName = "nvarchar(1000)")]
        public string? Description { get; set; }


        [StringLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string? Mobile { get; set; }
        
        [StringLength(200)]
        [Column(TypeName = "varchar(200)")]
        public string? Avatar { get; set; }

        [StringLength(200)]
        [Column(TypeName = "varchar(200)")]
        public string? BannerUri { get; set; }

        public bool PasswordChangeRequired { get; set; }

        [StringLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string? ClientId { get; set; }
        public bool? IsLoginFirstTime { get; set; }
        public bool? IsOnline { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ExpiryDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? LastLoginDate { get; set; }

        [DataType(DataType.Date)]
        private DateTime? createdDate { get; set; }
        public DateTime? CreateDate
        {
            get { return createdDate ?? DateTime.UtcNow; }
            set { createdDate = value; }
        }
        [DataType(DataType.Date)]
        public DateTime? ModifiedDate { get; set; }

        [JsonIgnore]
        [DefaultValue(true)]
        public bool? IsActive { get; set; }

        [JsonIgnore]
        [DefaultValue(false)]
        public bool? IsDeleted { get; set; }
        //[JsonIgnore]
        //public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }

    //public class ApplicationRole : IdentityUser
    //{
    //    public ApplicationRole() : base()
    //    {

    //    }
    //}
    //public class ApplicationUserRole : IdentityUserRole<string>
    //{
    //    public virtual ApplicationUser User { get; set; }
    //    //public virtual ApplicationRole Role { get; set; }
    //}
}
