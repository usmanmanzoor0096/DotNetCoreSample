using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AuthService.Models.RequestModels
{
    public class UserProfileUpdateRequest
    {

        [Required(ErrorMessage = "Fist name is required")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string? LastName { get; set; }
        public string? Description { get; set; }
        public string UserAvatarUri { get; set; }
        public string BannerUri { get; set; }
       // public UserProfileMediaFileRequest userProfileMediaFileRequest { get; set; }
    }

    public class UserProfileMediaFileRequest
    {
        public IFormFile AvatarFile { get; set; }
        public IFormFile BannerFile { get; set; }
    }
     
}
