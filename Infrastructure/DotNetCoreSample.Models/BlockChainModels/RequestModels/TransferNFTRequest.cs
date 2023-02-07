namespace  AuthService.Common.Models.BlockChainModels.RequestModels
{
    public class TransferNFTRequest
    {
        public string TemplateId { get; set; }
        public string RecieverAccount { get; set; }
        public string SenderAccount { get; set; }
        public string DropId { get; set; }
        public string MintNumber { get; set; }
        public double DropFlowPrice { get; set; }
        public double DropUsdPrice { get; set; }
        public string Message { get; set; }
    }
}
