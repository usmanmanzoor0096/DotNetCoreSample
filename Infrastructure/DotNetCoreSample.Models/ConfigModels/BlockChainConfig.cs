namespace  AuthService.Common.Models.ConfigModels
{
    public class BlockChianConfig
    {
        public string PrivateKey { get; set; }
        public string AccountAddress { get; set; }
        public string ContractAddress { get; set; }
        public string NFTContract { get; set; }
        public string NowWhereContract { get; set; }
        public string TemplateCreatedEvent { get; set; }
        public string NonFungibleToken { get; set; }
        public string FlowNetworkUrl { get; set; }
    }
}
