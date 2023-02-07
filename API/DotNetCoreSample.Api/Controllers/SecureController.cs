using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net; 
using  AuthService.Common.Models.Helper;
using AuthService.Common.Operation;
using AuthService.Models.DBModels;

namespace AuthService.Controllers
{
    [Authorize]
    public class SecureController : Controller
    {
        public string Token
        {
            get
            {
                return this.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            }
        }
        public string UserId
        {
            get
            {
                var data = this.HttpContext.Items["User"];
                if (data != null && !string.IsNullOrEmpty(((ApplicationUser)data).Id))
                    return ((ApplicationUser)data).Id;
                return "";
            }
        }

        public string ClientId
        {
            get
            {
                var data = this.HttpContext.Items["User"];
                if (data != null && !string.IsNullOrEmpty(((ApplicationUser)data).ClientId))
                    return ((ApplicationUser)data).Id;
                return "";
            }
        }

        protected IActionResult GenericApiResponse<T>(T payload, string message, HttpStatusCode responseType, Exception exception = null, bool isSucces = true)
        {
            ApiResponse<T> apiResponse = new ApiResponse<T>();
            apiResponse.StatusCodeValue = (int)responseType;

            if (responseType == HttpStatusCode.ExpectationFailed)
            {
                //apiResponse.Exception = exception;
                apiResponse.ExceptionStackTrace = exception == null ? null:  exception.StackTrace.ToString();
                apiResponse.StatusCode = HttpStatusCode.ExpectationFailed;
                apiResponse.Message = message;
                return BadRequest(apiResponse);
            }
            else if (responseType == HttpStatusCode.BadRequest)
            {
                apiResponse.StatusCode = HttpStatusCode.BadRequest;
                apiResponse.Success = false;
                apiResponse.Message = message;
                return BadRequest(apiResponse);
            }
            else if (responseType == HttpStatusCode.Forbidden)
            {
                apiResponse.StatusCode = HttpStatusCode.Forbidden;
                apiResponse.Success = false;
                apiResponse.Message = message;
                return BadRequest(apiResponse);
            }
            else if (responseType == HttpStatusCode.NoContent)
            {
                apiResponse.StatusCode = HttpStatusCode.NoContent;
                apiResponse.Success = false;
                apiResponse.Message = message;
                return BadRequest(apiResponse);
            }
            else if (responseType == HttpStatusCode.NotFound)
            {
                apiResponse.StatusCode = HttpStatusCode.NotFound;
                apiResponse.Success = false;
                apiResponse.Message = message;
                return BadRequest(apiResponse);
            }
            else
            {
                apiResponse.Payload = payload;
                apiResponse.Exception = null;
                apiResponse.StatusCode = HttpStatusCode.OK;
                apiResponse.Success = isSucces;
                apiResponse.Message = string.IsNullOrEmpty(message) ? "Success" : message;
                return Ok(apiResponse);
            }
        }

        protected IActionResult ValidateResponce<T>(OpResult<T> result, string message = null)
        {
            if (result.Succeeded)
                return GenericApiResponse(result, message == null ? "" : message, HttpStatusCode.OK, null, true);
            else
                return GenericApiResponse(default(IActionResult), "", HttpStatusCode.ExpectationFailed, result.Exception, false);
        }
    }
}
