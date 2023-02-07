using AuthService.Common.Models.DBModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Models.DBModels
{
    public class RefreshToken : BaseEntity
    { 
        [StringLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string? UserId { get; set; }
        
        [StringLength(1000)]
        [Column(TypeName = "nvarchar(1000)")]
        public string? Token { get; set; }
        
        [StringLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string Email { get; set; }

        public bool Expired { get; set; }
    }
}
