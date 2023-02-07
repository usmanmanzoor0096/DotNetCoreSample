using Amazon.Runtime;
using Amazon.S3;
using Amazon.SQS;
using Amazon.SQS.Model;
using AuthService.Models.AWS.DTOs;
using AuthService.Models.Config;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Communication.SQService
{
    public class SQSEmailService: ISQSEmailService
    {
        private readonly AWSQSConfigs _aWSQSConfigs;

        public SQSEmailService(IOptions<AWSQSConfigs> aWSQSConfigs)
        {
            _aWSQSConfigs = aWSQSConfigs.Value;
        }
        public async Task<bool> SendEmailViaSQS(SqsMessageModel sQSMessageModel)
        {
            var credentials = new BasicAWSCredentials(_aWSQSConfigs.AccessKey, _aWSQSConfigs.SecretKey);
            var client = new AmazonSQSClient(credentials, Amazon.RegionEndpoint.USEast1);
          
            string messagebody = JsonConvert.SerializeObject(sQSMessageModel);
            var request = new SendMessageRequest()
            {
                QueueUrl = _aWSQSConfigs.QueueURL,
                MessageBody = messagebody
            };
            await client.SendMessageAsync(request);
            return true;
        }
    }
}
