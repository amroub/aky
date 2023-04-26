using Akeneo.Client;

namespace Akeneo.Exceptions
{
    public class NotSupportedActionException : System.Exception
    {
        public NotSupportedActionException(string message) : base(message)
        {
        }
    }
}
