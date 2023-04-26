using System;

namespace aky.Foundation.Ddd.Infrastructure.Exceptions
{
    public class ServiceLocatorException : Exception
    {
        public ServiceLocatorException()
            : base($"CommonServiceLocator.ServiceLocator is not set. Please set common service locator.")
        {

        }

        public ServiceLocatorException(string message)
            : base(message)
        {
        }

        public ServiceLocatorException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
