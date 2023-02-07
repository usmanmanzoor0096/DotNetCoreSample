
namespace AuthService.Models.AWS.DTOs
{
    public class SqsMessageModel
    {
        public string TemplateType { get; set; }
        public List<string> Recipients { get; set; }
        public string Link { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public string Email { get; set; }
    }
}
