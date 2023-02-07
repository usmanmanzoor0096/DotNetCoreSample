using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AuthService.Common.Models.DBModels
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        //public Guid Id
        //{
        //    get { return id = Guid.NewGuid(); }
        //    set { id = value; }
        //}

        [DataType(DataType.Date)]
        private DateTime? createdDate { get; set; }
        public DateTime? CreateDate
        {
            get { return createdDate ?? DateTime.UtcNow; }
            set { createdDate = value; }
        }
        [DataType(DataType.Date)]
        public DateTime? ModifiedDate { get; set; }

        [DefaultValue(true)]
        public bool? IsActive { get; set; }

        [DefaultValue(false)]
        public bool? IsDeleted { get; set; }

    }
}
