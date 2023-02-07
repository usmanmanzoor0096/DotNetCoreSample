using System;

namespace AuthService.Models.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException() : base("Operation failed. Please try again later")
        {
        }

        public BusinessException(Exception ex) : base(ex.Message, ex.InnerException)
        {
        }

        public BusinessException(string message) : base(message)
        {
        }

        public BusinessException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}