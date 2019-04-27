using System;

namespace Conformity
{
    internal class InvalidJobException : Exception
    {
        public InvalidJobException(string message)
            : this(message, null)
        {
        }

        public InvalidJobException(string message, Exception innerException)
            : base("Invalid Job: " + message, innerException)
        {
        }
    }
}
