using System;

namespace AuthService.Models.Exceptions
{
    public class UnauthorizedLockException : Exception
    {
        public UnauthorizedLockException(string message) : base(message)
        {
        }
    }
}