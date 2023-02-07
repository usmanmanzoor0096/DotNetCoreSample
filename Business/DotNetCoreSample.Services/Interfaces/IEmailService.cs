using AuthService.Common.Operation;
using AuthService.Models.Dtos;
using AuthService.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Services.Interfaces
{
    public interface IEmailService
    {
        /// <summary>
        /// IUserService Interface : Send email for password reset. Initiate recover password process
        /// </summary>
        /// <param name="emaiData"></param>
        /// <returns></returns>
        Task<bool> SendForgetpasswordEmailAsync(EmailDto emaiData);

        Task<bool> SendSignupSuccessfulEmailAsync(EmailDto emaiData);

        Task<bool> SendChangeResetSuccessfulEmailAsync(EmailDto emaiData);
        Task<string> GetEmailTemplateFromDb(string templateName);
        Task<bool> RegisterClientEmail(EmailDto emailDto);
    }
}
