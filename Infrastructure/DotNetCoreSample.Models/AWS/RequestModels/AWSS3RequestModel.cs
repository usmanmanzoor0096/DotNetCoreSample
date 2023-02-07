using Microsoft.AspNetCore.Http;

namespace  AuthService.Common.Models.AWS.RequestModels
{
    public class AWSS3RequestModel
    {
        public string BucketName { get; set; }
        public string Key { get; set; }
        public string Prefix { get; set; }
        public IFormFile File { get; set; }
    }
}
