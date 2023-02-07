using  AuthService.Common.Models.EmailModels.DtoModels;

namespace  AuthService.Common.Communication.EmailServices
{
    public interface IEmailSendGrid
    {
        Task SendEmailAsync(SendEmailModel sendEmailModel);

        Task SendExceptionEmail(SendEmailModel sendEmailModel);
    }
}
