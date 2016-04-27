using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using GlobalLogger.Enums;
using GlobalLogger.Exceptions;
using GlobalLogger.Properties;
using NLog;
using NLog.Config;

namespace GlobalLogger
{
    public enum Level
    {
        Trace,
        Debug,
        Info,
        Warning,
        Error,
        Fatal
    }

    /// <summary>
    /// Main logger class. This class's instance can be created as the logger object.
    /// </summary>
    public class GlobalLogger
    {
        #region Configuration
        private const string ConfigFilesPath = "LogConfigurations";

        /// <summary>
        /// Static constructor - called once, before the first use of GlobalLogger. Sets all fixed logger's properties.
        /// </summary>
        static GlobalLogger()
        {
            var mainConfigFile = String.Empty;

            // get the main configuration file
            if (LogManager.Configuration != null)
                mainConfigFile = LogManager.Configuration.FileNamesToWatch.FirstOrDefault();
            
            // check if additional config files might be available
            var configFilesFolderExist = Directory.Exists(ConfigFilesPath);

            var additionalConfigFiles = new List<string>(4);
            if (configFilesFolderExist)
            {
                additionalConfigFiles = Directory.GetFiles(ConfigFilesPath)
                                                 .Where(filename => !String.IsNullOrEmpty(filename))
                                                 .Where(filename => Path.GetExtension(filename).Equals(".config")).ToList();
            }

            var otherConfigFileExist = configFilesFolderExist && additionalConfigFiles.Any();

            // if no config file could be found (main or additional), report critical config issue
            if (String.IsNullOrEmpty(mainConfigFile) && !otherConfigFileExist)
                throw new NLogConfigurationException(string.Format(Resources.ErrorNLogInitFailWrongConfiguration, ConfigFilesPath));
            
            List<string> filesToBeIncluded;
            if (!String.IsNullOrEmpty(mainConfigFile))
            {
                // here check that every additional config is loaded (include tag in the main config file)
                if (otherConfigFileExist)
                    filesToBeIncluded = GetFilesToBeIncluded(additionalConfigFiles);
                else
                    return; // no additional config file to be loaded, initialization of NLog is complete!
            }
            else
            {
                // There is no main config file yet. Set the first additional file as if it was the main one
                mainConfigFile = additionalConfigFiles.First();
                LogManager.Configuration = new XmlLoggingConfiguration(mainConfigFile);

                // here check that every additional config is loaded (include tag in the main config file)
                filesToBeIncluded = GetFilesToBeIncluded(additionalConfigFiles);
            }

            // Add include tags to the main config file for the missing additional config files
            IncludeAdditionalConfigFiles(mainConfigFile, filesToBeIncluded);
            // and reload configuration from file
            LogManager.Configuration = new XmlLoggingConfiguration(mainConfigFile);
        }

        /// <summary>
        /// Adds include statements into main config file for other configurations files to be used
        /// </summary>
        /// <param name="mainConfigFile">Main config file name</param>
        /// <param name="filesToBeIncluded">List of additional config file names</param>
        private static void IncludeAdditionalConfigFiles(string mainConfigFile, List<string> filesToBeIncluded)
        {
            // if the main config file is from the additional config files folder, specific case
            var directory = Path.GetDirectoryName(mainConfigFile);
            var isMainConfigFileFromConfigFilesFolder = directory.Equals(ConfigFilesPath);
            
            var document = XDocument.Load(mainConfigFile);
            var rootElement = document.FirstNode as XElement;
            if (rootElement == null || !rootElement.Name.LocalName.Equals("nlog"))
                throw new NLogConfigurationException(string.Format(Resources.ErrorNLogElementNotFound, mainConfigFile));

            foreach (var filename in filesToBeIncluded)
            {
                var formattedFilename = filename;

                // if the current file is the main config file itself, skip the include
                if (isMainConfigFileFromConfigFilesFolder && Path.GetFileName(mainConfigFile).Equals(Path.GetFileName(formattedFilename)))
                    continue;

                // remove ConfigFilesPath from the filename if it comes from the same folder as the main config file
                if (isMainConfigFileFromConfigFilesFolder && Path.GetDirectoryName(formattedFilename).Equals(ConfigFilesPath))
                    formattedFilename = Path.GetFileName(formattedFilename);

                rootElement.Add(new XElement("include", new XAttribute("file", formattedFilename)));
            }

            document.Save(mainConfigFile);
        }

        /// <summary>
        /// Compares the list of additional files to be included into main config file giving the list of non yet included config files
        /// </summary>
        /// <param name="additionalConfigFiles">Additional config files to be included into main config file</param>
        /// <returns>List of additional config files not yet included into main config file</returns>
        private static List<string> GetFilesToBeIncluded(List<string> additionalConfigFiles)
        {
            var filesToBeIncluded = new List<string>();
            filesToBeIncluded.AddRange(additionalConfigFiles.Where(additionalConfigFile => !LogManager.Configuration.FileNamesToWatch.Contains(additionalConfigFile)));

            return filesToBeIncluded;
        }
        #endregion Configuration

        private readonly ILogger _logger;
        public string Name { get; private set; }

        #region Persistent and volatile properties

        private Dictionary<string, Object> _persistentProperties;
        private Dictionary<string, Object> _volatileProperties;

        #endregion Persistent and volatile Properties

        #region Other properties

        private Dictionary<string, Stopwatch> TimeStopwatches = new Dictionary<string, Stopwatch>();
        #endregion Other properties
        
        /// <summary>
        /// Get a new logger instance called after the calling class
        /// </summary>
        public GlobalLogger()
        {
            var callingFrame = new StackFrame(1, false);
            var callingType = callingFrame.GetMethod().DeclaringType.FullName;

            InitializePropertiesContainers();

            Name = callingType;
            _logger = LogManager.GetLogger(callingType);
        }

        /// <summary>
        /// Get a new logger instance with the given name
        /// </summary>
        public GlobalLogger(string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(Resources.ErrorGlobalLoggerNameMandatory);

            InitializePropertiesContainers();

            Name = name;
            _logger = LogManager.GetLogger(name);
        }

        /// <summary>
        /// Get a new logger instance called after the object passed as parameter
        /// </summary>
        public GlobalLogger(Object type)
        {
            if (type == null)
                throw new ArgumentNullException(Resources.ErrorGlobalLoggerNullObjectInit);

            InitializePropertiesContainers();

            Name = type.GetType().FullName;
            _logger = LogManager.GetLogger(Name);
        }

        private void InitializePropertiesContainers()
        {
            _persistentProperties = new Dictionary<string, object>();
            _volatileProperties = new Dictionary<string, object>();
        }

        internal void AddOrUpdateProperty(string name, Object value, bool isPersistent)
        {
            AddOrUpdateProperty(name, value, isPersistent ? _persistentProperties : _volatileProperties);
        }

        private void AddOrUpdateProperty(string name, object value, Dictionary<string, object> propertiesContainer)
        {
            if (propertiesContainer.ContainsKey(name))
                propertiesContainer[name] = value;
            else
                propertiesContainer.Add(name, value);
        }

        /// <summary>
        /// Clears the volatile properties list of the logger
        /// </summary>
        public void ClearVolatileProperties()
        {
            _volatileProperties.Clear();
        }

        /// <summary>
        /// Clears the persistent properties list of the logger
        /// </summary>
        public void ClearPersistentProperties()
        {
            _persistentProperties.Clear();
        }

        /// <summary>
        /// Logs a message with LogLevel set as Fatal
        /// </summary>
        /// <param name="message">Message content to be logged</param>
        public void LogFatal(string message)
        {
            Log(LogLevel.Fatal, message);
        }


        /// <summary>
        /// Logs a message with LogLevel set as Error
        /// </summary>
        /// <param name="message">Message content to be logged</param>
        public void LogError(string message)
        {
            Log(LogLevel.Error, message);
        }

        /// <summary>
        /// Logs a message with LogLevel set as Warn (Warning)
        /// </summary>
        /// <param name="message">Message content to be logged</param>
        public void LogWarning(string message)
        {
            Log(LogLevel.Warn, message);
        }

        /// <summary>
        /// Logs a message with LogLevel set as Info
        /// </summary>
        /// <param name="message">Message content to be logged</param>
        public void LogInfo(string message)
        {
            Log(LogLevel.Info, message);
        }

        /// <summary>
        /// Logs a message with LogLevel set as Debug
        /// </summary>
        /// <param name="message">Message content to be logged</param>
        public void LogDebug(string message)
        {
            Log(LogLevel.Debug, message);
        }

        /// <summary>
        /// Logs a message with LogLevel set as Trace
        /// </summary>
        /// <param name="message">Message content to be logged</param>
        public void LogTrace(string message)
        {
            Log(LogLevel.Trace, message);
        }

        private void Log(LogLevel level, string message)
        {
            var logInfo = new LogEventInfo(level, _logger.Name, message);

            // load persistent keys
            foreach (var property in _persistentProperties)
            {
                logInfo.Properties[property.Key] = property.Value.ToString();
            }

            // load volatile keys
            foreach (var property in _volatileProperties)
            {
                logInfo.Properties[property.Key] = property.Value.ToString();
            }

            _logger.Log(typeof(GlobalLogger), logInfo);

            // clear those properties after each log
            ClearVolatileProperties();
        }

        #region Time measurement

        /// <summary>
        /// Creates and starts timer by it's unique name specified as a parameter
        /// </summary>
        /// <param name="timerName">Unique name of the timer</param>
        public void StartTimer(string timerName)
        {
            if (String.IsNullOrEmpty(timerName))
                throw new GlobalLoggerException("Timer name must be specified!");

            if(!TimeStopwatches.ContainsKey(timerName))
                TimeStopwatches.Add(timerName, new Stopwatch());

            var stopwatch = TimeStopwatches[timerName];
            
            stopwatch.Start();
        }

        /// <summary>
        /// Stops timer given by it's name as a parameter and logs the measured time
        /// </summary>
        /// <param name="timerName">Unique name of the timer</param>
        /// <param name="logLevel">Log level for logging the measured time</param>
        public void StopTimer(string timerName, Level logLevel)
        {
            if (String.IsNullOrEmpty(timerName))
                throw new GlobalLoggerException("Timer name must be specified!");

            if (!TimeStopwatches.ContainsKey(timerName))
                throw new GlobalLoggerException(String.Format("Timer {0} does not exist. It must be initialized with logger.StartTimer() before!", timerName));

            var stopwatch = TimeStopwatches[timerName];

            stopwatch.Stop();

            // default log message if not specified
            var message = "[" + timerName + "]" + " Time measured: " + stopwatch.ElapsedMilliseconds + " [ms]";

            StopTimer(logLevel, message);
        }

        /// <summary>
        /// Stops timer given by it's name as a parameter and logs the measured time
        /// </summary>
        /// <param name="timerName">Unique name of the timer</param>
        /// <param name="logLevel">Log level for logging the measured time (Level.Trace if not specified)</param>
        /// <param name="messageString">Message which will be logged (requires {0} for the time value)</param>
        /// <param name="timeUnit">TimeUnit to specify the precision of measured time to be logged</param>
        /// <param name="showTimerName">If set to true, "[TimerName]" tag will be added in the beginning of log message</param>
        public void StopTimer(string timerName, Level logLevel, String messageString, TimeUnit timeUnit, bool showTimerName = false)
        {
            if (String.IsNullOrEmpty(timerName))
                throw new GlobalLoggerException("Timer name must be specified!");

            if (!TimeStopwatches.ContainsKey(timerName))
                throw new GlobalLoggerException(String.Format("Timer {0} does not exist. It must be initialized with logger.StartTimer() before!", timerName));

            var stopwatch = TimeStopwatches[timerName];

            stopwatch.Stop();
            // default log message if not specified
            var message = new StringBuilder();


            // build the log message
            if (showTimerName)
                message.Append("[" + timerName + "]: ");

            var measuredTime = String.Empty;

            switch (timeUnit)
            {
                case TimeUnit.Days:
                    measuredTime = stopwatch.Elapsed.Days + " [d]";
                    break;

                case TimeUnit.Hours:
                    measuredTime = stopwatch.Elapsed.Hours + " [h]";
                    break;


                case TimeUnit.Minutes:
                    measuredTime = stopwatch.Elapsed.Minutes + " [m]";
                    break;


                case TimeUnit.Seconds:
                    measuredTime = stopwatch.Elapsed.Seconds + " [s]";
                    break;


                case TimeUnit.Miliseconds:
                    measuredTime = stopwatch.Elapsed.Milliseconds +  "[ms]";
                    break;


                case TimeUnit.Ticks:
                    measuredTime = stopwatch.Elapsed.Ticks + " [ticks]";
                    break;
            }

            message.Append(String.Format(messageString, measuredTime));


            StopTimer(logLevel, message.ToString());
        }

        private void StopTimer(Level logLevel, string message)
        {
            switch (logLevel.ToString())
            {
                case "Trace":
                    LogTrace(message);
                    break;

                case "Debug":
                    LogDebug(message);
                    break;

                case "Info":
                    LogInfo(message);
                    break;

                case "Warning":
                    LogWarning(message);
                    break;

                case "Error":
                    LogError(message);
                    break;

                case "Fatal":
                    LogFatal(message);
                    break;
            }

        }
        #endregion Time measurement

        #region Exception Handling
        /// <summary>
        /// Logs an exception with an optional additional message.
        /// The default log level, if none provided, is 'Error'.
        /// </summary>
        /// <param name="exception"> The exception to be logged. </param>
        /// <param name="additionalMessage"> An optional message to accompany the exception. </param>
        /// <param name="level"> The level at which the exception and message will be logged. </param>
        public void LogException(Exception exception, string additionalMessage = null, Level level = Level.Error)
        {
            switch (level)
            {
                case Level.Trace:
                    _logger.Trace(exception, additionalMessage);
                    break;
                case Level.Debug:
                    _logger.Debug(exception, additionalMessage);
                    break;
                case Level.Info:
                    _logger.Info(exception, additionalMessage);
                    break;
                case Level.Warning:
                    _logger.Warn(exception, additionalMessage);
                    break;
                case Level.Error:
                    _logger.Error(exception, additionalMessage);
                    break;
                case Level.Fatal:
                    _logger.Fatal(exception, additionalMessage);
                    break;
            }
        }
        #endregion Exception Handling
    }
}
