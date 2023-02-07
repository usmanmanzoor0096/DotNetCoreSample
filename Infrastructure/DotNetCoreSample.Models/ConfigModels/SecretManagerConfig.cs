using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  AuthService.Common.Models.ConfigModels
{
    public class SecretManagerConfig
    {
        public string? StagingConnectionString { get; set; }
        public string? DevConnectionString { get; set; }
        public string? ProdConnectionString { get; set; }
    }
}
