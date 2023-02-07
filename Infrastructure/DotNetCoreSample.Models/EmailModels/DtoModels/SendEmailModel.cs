namespace  AuthService.Common.Models.EmailModels.DtoModels
{
    public class SendEmailModel
    {
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string PlainTextMessage { get; set; }
        public Dictionary<string, string> File { get; set; }
    }
}
