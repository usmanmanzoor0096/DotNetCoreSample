using AuthService.Common.Models.ConfigModels; 

namespace AuthService.Models.Config
{
    //TODO: Need to set this up
    public class AppSettings
    {
        public AppSettings()
        {
        }

        public SendGridAuthOptions SendGridAuthOptions { get; set; }
        public TwilioAuthOptions TwilioAuthOptions { get; set; }
        public EmailStaticData EmailStaticData { get; set; }
        public BlobStorageConfig BlobStorageConfig { get; set; }
        public ArweaveData ArweaveData { get; set; }
    }

    public class SendGridAuthOptions
    {
        public string SendGridUser { get; set; }
        public string SendGridFrom { get; set; }
        public string SendGridKey { get; set; }
    }

    public class TwilioAuthOptions
    {
        public string AccountSID { get; set; }
        public string AuthToken { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class EmailStaticData
    {
        public string Url { get; set; }
        public string ExpireyTimeSpan { get; set; }
        public string FlowTransactionUrl { get; set; }
        public string EmailPlaceHolderImg { get; set; }
    }
    public class ArweaveData
    {
        public string ArweaveStaticUrl { get; set; }
    }
}
