using  AuthService.Common.Models.ConfigModels;
using  AuthService.Common.Models.EmailModels.DtoModels;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace  AuthService.Common.Communication.EmailServices
{
    public class EmailSendGrid : IEmailSendGrid
    {
        private readonly SendGridConfig _sendGridConfig;

        public EmailSendGrid(IOptions<SendGridConfig> sendGridConfig)
        {
            _sendGridConfig = sendGridConfig.Value;
        }

        public async Task SendEmailAsync(SendEmailModel sendEmailModel)
        {
            var client = new SendGridClient(_sendGridConfig.SendGridKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_sendGridConfig.SendGridFrom, _sendGridConfig.SendGridUser),
                Subject = sendEmailModel.Subject,
                PlainTextContent = sendEmailModel.Message,
                HtmlContent = sendEmailModel.Message
            };
            msg.AddTo(new EmailAddress(sendEmailModel.Email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            await client.SendEmailAsync(msg);
        }

        public async Task SendExceptionEmail(SendEmailModel sendEmailModel)
        {
            var client = new SendGridClient(_sendGridConfig.SendGridKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_sendGridConfig.SendGridFrom, "TroonNFT Exceptions"),
                Subject = sendEmailModel.Subject,
                PlainTextContent = sendEmailModel.PlainTextMessage,
                HtmlContent = sendEmailModel.Message,
            };
            msg.AddTo(sendEmailModel.Email);
            foreach (var item in sendEmailModel.File)
            {
                msg.AddAttachment(item.Key, item.Value, null, null, null);
            }

            await client.SendEmailAsync(msg);
        }
    }
}
