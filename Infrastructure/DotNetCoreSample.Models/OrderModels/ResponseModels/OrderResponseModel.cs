using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  AuthService.Common.Models.OrderModels.ResponseModels
{
    public class OrderResponseModel
    {
        public Guid Uuid { get; set; }
        public string ClientId { get; set; }
        public string DropId { get; set; }
        public string ArtWorkId { get; set; }
        public int Quantity { get; set; }
        public decimal USDPrice { get; set; }
        public decimal CryptoPrice { get; set; }
        public string? PaymentType { get; set; }
        public bool IsPaymentSucceeded { get; set; }
        public bool? NFTTransferred { get; set; }
        public int? OrderStatus { get; set; }
    }
}
