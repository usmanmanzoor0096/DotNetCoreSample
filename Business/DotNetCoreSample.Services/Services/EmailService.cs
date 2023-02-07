using AuthService.Common.Communication.EmailServices;
using AuthService.Common.Models.ConfigModels;
using AuthService.Common.Operation;
using AuthService.Communication.SQService;
using AuthService.Data.IRepositories;
using AuthService.Models.AWS.DTOs;
using AuthService.Models.DBModels;
using AuthService.Models.Dtos;
using AuthService.Models.Enum;
using AuthService.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
namespace AuthService.Services.Services
{
    public class EmailService : IEmailService
    {
        private readonly IUserRepository _userRepository;
        private readonly IDBOperationExecutor _dbOperationExecutor;
        private readonly ISafeOperationExecutor _operationExecutor;
        private readonly IEmailSendGrid _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppConfig _appConfig;
        private readonly ISQSEmailService _sQSEmailService;

        public EmailService(
            IUserRepository userRepository,
            IDBOperationExecutor dbOperationExecutor,
            ISafeOperationExecutor safeOperationExecutor,
            IEmailSendGrid emailSender,
            UserManager<ApplicationUser> userManager,
            AppConfig appConfig,
            ISQSEmailService sQSEmailService
            )
        {
            _userRepository = userRepository;
            _dbOperationExecutor = dbOperationExecutor;
            _operationExecutor = safeOperationExecutor;
            _emailSender = emailSender;
            _userManager = userManager;
            _appConfig = appConfig;
            _sQSEmailService = sQSEmailService;
        }


        public Task<string> GetEmailTemplateFromDb(string templateName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendChangeResetSuccessfulEmailAsync(EmailDto emaiData)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SendForgetpasswordEmailAsync(EmailDto emailDto)
        {
            try
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(emailDto.User);
                var callbackUrl = string.IsNullOrEmpty(emailDto.SiteUrl) ? _appConfig.BaseUri + "/set-password" : emailDto.SiteUrl
                                  + "?userId="
                                  + emailDto.User.Id + "&code="
                                  + WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                List<string> recipients = new List<string>();
                recipients.Add(emailDto.EmailAddress);
                SqsMessageModel sqsMessageModel = new SqsMessageModel
                {
                    Subject = "Reset Password",
                    Link = callbackUrl,
                    Recipients = recipients,
                    TemplateType = EmailTemplate.ResetPasswordTemplate.ToString()
                };
                await _sQSEmailService.SendEmailViaSQS(sqsMessageModel);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<bool> RegisterClientEmail(EmailDto emailDto)
        {
            try
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(emailDto.User);
                var callbackUrl = _appConfig.BaseUri+ "/set-password"
                                  + "?userId="
                                  + emailDto.User.Id + "&code="
                                  + WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                List<string> recipients = new List<string>();
                recipients.Add(emailDto.EmailAddress);
                SqsMessageModel sqsMessageModel = new SqsMessageModel
                {
                    Subject = "Set Your Password",
                    Link = callbackUrl,
                    Recipients = recipients,
                    TemplateType = EmailTemplate.CongragulationTemplate.ToString(),
                    Email = emailDto.EmailAddress

                };
                await _sQSEmailService.SendEmailViaSQS(sqsMessageModel);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public Task<bool> SendSignupSuccessfulEmailAsync(EmailDto emaiData)
        {
            throw new NotImplementedException();
        }
    }
}
