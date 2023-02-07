using AuthService.Common.GenericResponses;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using System.Net;
using static AuthService.Common.Enums.APIEnums;

namespace AuthService.API.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        /// <summary>
        /// This extension method is used as global exception handler and logs the exceptions on SEQ
        /// This extension method is created for WebApplication type. And register as a middle ware in program.cs
        /// </summary>
        /// <param name="app"></param>
        /// <param name="logger"></param>
        public static void ConfigureExceptionHandler(this WebApplication app, ILogger logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        logger.LogError(contextFeature.Error.ToString());
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(GenericApiResponse<bool>.Failure(contextFeature.Error.ToString(), APIStatusCodes.InternalServerError)));
                    }
                });
            });
        }
    }
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
