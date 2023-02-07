using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Models.Config
{
    public class AWSQSConfigs
    {
        public string QueueURL { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string Region { get; set; }
    }
}
