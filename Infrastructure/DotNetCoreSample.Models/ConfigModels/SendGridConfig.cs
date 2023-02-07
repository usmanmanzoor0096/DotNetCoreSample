namespace  AuthService.Common.Models.ConfigModels
{
    public class SendGridConfig
    {
        public string SendGridUser { get; set; }
        public string SendGridFrom { get; set; }
        public string DevsMailingAddress { get; set; }
        public string SendGridKey { get; set; }
    }
}
