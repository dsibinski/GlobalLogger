using System;

namespace GlobalLogger
{
    /// <summary>
    /// Provides the FluentAPI methods for LogHelper.
    /// </summary>
    public static class LogHelperFluentApi
    {
        /// <summary>
        /// Add a new persistent property to the logger or update it if it already exists. Persistent properties live in the logger until they are explicity cleared.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="name"> Name of the property to be added or updated. </param>
        /// <param name="value"> Value of the property to be added or updated. </param>
        /// <returns></returns>
        public static LogHelper AddOrUpdatePersistentProperty(this LogHelper logger, string name, Object value)
        {
            logger.AddOrUpdateProperty(name, value, true);
            return logger;
        }

        /// <summary>
        /// Add a new volatile property to the logger or update it if it already exists. Volatile properties live in the logger until the next message log.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="name"> Name of the property to be added or updated. </param>
        /// <param name="value"> Value of the property to be added or updated. </param>
        /// <returns></returns>
        public static LogHelper AddOrUpdateVolatileProperty(this LogHelper logger, string name, Object value)
        {
            logger.AddOrUpdateProperty(name, value, false);
            return logger;
        }
    }
}
