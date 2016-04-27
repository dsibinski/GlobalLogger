using System;

namespace GlobalLogger.Exceptions
{
    /// <summary>
    /// Custom exception thrown by GlobalLogger
    /// </summary>
    public class GlobalLoggerException : Exception
    {
        /// <summary>
        /// Creates an empty GlobalLoggerException
        /// </summary>
        public GlobalLoggerException()
        {
        }

        /// <summary>
        /// Creates GlobalLoggerException with its message set
        /// </summary>
        /// <param name="message">Exception's message content</param>
        public GlobalLoggerException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates GlobalLoggerException with its message and inner exception set
        /// </summary>
        /// <param name="message">Exception's message content</param>
        /// <param name="inner">Exception's inner exception entity</param>
        public GlobalLoggerException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

}
