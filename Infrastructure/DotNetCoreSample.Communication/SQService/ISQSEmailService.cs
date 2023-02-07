using AuthService.Models.AWS.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Communication.SQService
{
    public interface ISQSEmailService
    {
        Task<bool> SendEmailViaSQS(SqsMessageModel sQSMessageModel);
    }
}
