using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  AuthService.Common.Models.OrderModels.RequestModels
{
    public class OrderRequestModel
    {
        public string ClientId { get; set; }
        public string DropId { get; set; }
        public string ArtWorkId { get; set; }
        public int Quantity { get; set; }
        public decimal USDPrice { get; set; }
        public decimal CryptoPrice { get; set; }

    }
}
