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
using NLog.Targets;

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
    /// Main logger class. This class's instance can be created as the logger object .
    /// </summary>
    public class LogHelper
    {
        #region Configuration
        private const string ConfigFilesPath = "LogConfigurations";
        private static string AdditionalConfigsPath;
        /// <summary>
        /// Static constructor - called once, before the first use of LogHelper. Sets all fixed logger's properties.
        /// </summary>
        static LogHelper()
        {
            

            #region OBSOLETE
            // [DS] Decided not to use main config file anymore. GlobalLogger is dedicates solution, and it may cause
            // [DS] many problems if e.g. NLog.config file contains variables set in the external code, not in the config itself.
            // [DS] GlobalLogger is then not able to parse such config file.
            // [DS] GlobalLogger will now only use configs located in LogConfigurations folder.
            // get the main configuration file
          /*  var mainConfigFile = String.Empty;
            if (LogManager.Configuration != null)
                mainConfigFile = LogManager.Configuration.FileNamesToWatch.FirstOrDefault();*/
            #endregion OBSOLETE

            var isRunningFromExecutable = System.Reflection.Assembly.GetEntryAssembly() != null;

            AdditionalConfigsPath = isRunningFromExecutable
                ? (Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\" + ConfigFilesPath)
                : ConfigFilesPath;

            // check if additional config files might be available
            var configFilesFolderExist = Directory.Exists(AdditionalConfigsPath);

            var configFiles = new List<string>(4);
            if (configFilesFolderExist)
            {
                configFiles = Directory.GetFiles(AdditionalConfigsPath)
                                                 .Where(filename => !String.IsNullOrEmpty(filename))
                                                 .Where(filename => Path.GetExtension(filename).Equals(".config")).ToList();
            }

            var colfigFilesExist = configFilesFolderExist && configFiles.Any();

            // if no config file could be found (main or additional), report critical config issue
            if (!colfigFilesExist)
                throw new NLogConfigurationException(string.Format(Resources.ErrorNLogInitFailWrongConfiguration, AdditionalConfigsPath));



            List<string> filesToBeIncluded = new List<string>();

            // Get the first config file as the main one
            var mainConfigFile = configFiles.First();
           

            // here check that every additional config is loaded (include tag in the main config file)
            //filesToBeIncluded = GetFilesToBeIncluded(configFiles);
            filesToBeIncluded.AddRange(configFiles.Where(configFile => !configFile.Equals(mainConfigFile)));

            // Add include tags to the main config file for the missing additional config files
            IncludeAdditionalConfigFiles(mainConfigFile, filesToBeIncluded);
            // Set the LogManager configuration according to main config file (potentially updated with another included config files)
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
            var isMainConfigFileFromConfigFilesFolder = directory.Equals(AdditionalConfigsPath);
            
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
                if (isMainConfigFileFromConfigFilesFolder && Path.GetDirectoryName(formattedFilename).Equals(AdditionalConfigsPath))
                    formattedFilename = Path.GetFileName(formattedFilename);

                // Retrieve all included configs filenames
                var alreadyIncludedFileNames = rootElement.Elements("include").Attributes("file").Select(atr => atr.Value);

                // Include only if not included yet
                if(!alreadyIncludedFileNames.Contains(formattedFilename))
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
            var fileNamesToWatchLocalPaths = LogManager.Configuration.FileNamesToWatch.Select(GetConfigLocalFilePath);

            filesToBeIncluded.AddRange(additionalConfigFiles.Where(
                additionalConfigFile => !fileNamesToWatchLocalPaths.Contains(GetConfigLocalFilePath(additionalConfigFile))));

            return filesToBeIncluded;
        }

        /// <summary>
        /// Returns the local config file path (related to logger's directory), e.g. LogConfigurations\custom_config.config
        /// </summary>
        /// <param name="configFilePartOrFullPath">Path to the config file (part or full path)</param>
        /// <returns>Local config file path</returns>
        private static string GetConfigLocalFilePath(string configFilePartOrFullPath)
        {
            return AdditionalConfigsPath + "\\" + Path.GetFileName(configFilePartOrFullPath);
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
        public LogHelper()
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
        public LogHelper(string name)
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
        public LogHelper(Object type)
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

            _logger.Log(typeof(LogHelper), logInfo);

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

        #region Additional features

        /// <summary>
        /// Returns the filename of the logger file target with given name. It only works for FILE targets.
        /// Method sets the TimeStamp used in config file as current date and Name of the logger as the LogHelper's instance name.
        /// </summary>
        /// <param name="fileTargetName">Name of the File logger target</param>
        /// <returns>Name of the file to which given target writes</returns>
        public string GetFileTargetFileName(string fileTargetName)
        {
            var fileTarget = (FileTarget) LogManager.Configuration.FindTargetByName(fileTargetName);
            var logEventInfo = new LogEventInfo { TimeStamp = DateTime.Now, LoggerName = Name};
            return fileTarget.FileName.Render(logEventInfo);

        }
        #endregion Additional features
    }
}
