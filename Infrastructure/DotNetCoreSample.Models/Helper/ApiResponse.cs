using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace  AuthService.Common.Models.Helper
{
    public class ApiResponse<T> : ApiResponse
    {
        public T Payload { get; set; }

        public Exception Exception { get; set; }
        public string ExceptionStackTrace { get; set; }
    }

    public class ApiResponse
    {
        public ApiResponse()
        {
            Version = "1.0.0";
        }

        public string Version { get; set; }
        private HttpStatusCode _statusCode;
        private int _statusCodeValue;
        public int StatusCodeValue
        {
            get
            {
                return _statusCodeValue;
            }
            set
            {
                _statusCodeValue = value;
            }
        }
        public HttpStatusCode StatusCode
        {
            get
            {
                return _statusCode;
            }
            set
            {
                _statusCode = value;
                SetResponseMessage(value);
            }
        }

        public string Message { get; set; }
        public bool Success { get; set; }

        private void SetResponseMessage(HttpStatusCode StatusCode)
        {
            if (string.IsNullOrEmpty(this.Message))
            {
                switch (StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        this.Message = "User is not authorize to access this resource.";
                        this.Success = false;
                        break;

                    case HttpStatusCode.Forbidden:
                        this.Message = "Sorry! This feature is not available for trial user.";
                        this.Success = false;
                        break;

                    case HttpStatusCode.Created:
                        this.Message = "Item created successfully.";
                        this.Success = true;
                        break;

                    case HttpStatusCode.NoContent:
                        this.Message = "No content.";
                        this.Success = false;
                        break;

                    case HttpStatusCode.BadRequest:
                        this.Message = "Bad request.";
                        this.Success = false;
                        break;

                    case HttpStatusCode.NotFound:
                        this.Message = "Specified resource not found.";
                        this.Success = false;
                        break;

                    case HttpStatusCode.RequestTimeout:
                        this.Message = "Request time out.";
                        this.Success = false;
                        break;

                    case HttpStatusCode.InternalServerError:
                        this.Message = "Internal server error occurred.";
                        this.Success = false;
                        break;

                    case HttpStatusCode.ServiceUnavailable:
                        this.Message = "Service is not available.";
                        this.Success = false;
                        break;

                    case HttpStatusCode.OK:
                        this.Message = "";
                        this.Success = true;
                        break;

                    case HttpStatusCode.NonAuthoritativeInformation:
                        this.Message = "Couldn't succeeded transformation applied on item.";
                        this.Success = false;
                        break;

                    case HttpStatusCode.ExpectationFailed:
                        this.Message = "Exception occured while Processing your request";
                        this.Success = false;
                        break;

                    default:
                        this.Message = "";
                        break;
                }
            }
        }
    }
}
