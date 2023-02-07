using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  AuthService.Common.Models.BlockChainModels.RequestModels
{
    public class TransactionRequestModel
    {
        public string UserId { get; set; }
        public string TransactionId { get; set; }
        public string TransactionName { get; set; }
        public string StorageTransaction { get; set; }
        public string TransactionJson { get; set; }
    }
}
