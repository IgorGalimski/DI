using System;

namespace DI
{
    public class InjectException : Exception
    {
        public InjectException(string message) : base(message)
        {
        }
    }
}