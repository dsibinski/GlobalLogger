using System;
using System.Diagnostics;
using GlobalLogger.Enums;
using NLog;

namespace GlobalLogger.Demo.Models
{
    public class Employee
    {
        public string FirstName { get; protected set; }
        public string LastName { get; protected set; }
        public int Age { get; protected set; }
        public int BadgeNumber { get; protected set; }
        public string Position { get; protected set; }

        public Employee(string firstName, string lastName, int age, int badgeNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            BadgeNumber = badgeNumber;
            Position = "Employee";

            // test object ctor. TODO put this in a separate class for tests
            var logger1 = new GlobalLogger(this);
            logger1.LogTrace("new Employee created (custom ctor with object param)");

            logger1.StartTimer("Timer1");
            // some stuff....
            int a = age + 5;
            int b = 1 + a + 2;
            double aa = a/b;
            
           logger1.StopTimer("Timer1", Level.Trace, "Measured time was: {0}", TimeUnit.Seconds);

            // test string ctor. TODO put this in a separate class for tests
            var logger2 = new GlobalLogger("CustomLoggerName");
            logger2.LogTrace("new Employee created (custom ctor with string param)");

            // test fluent api to add properties
            var fluentLogger = new GlobalLogger();
            fluentLogger
                .AddOrUpdatePersistentProperty("firstName", FirstName)
                .AddOrUpdatePersistentProperty("lastName", LastName)
                .AddOrUpdateVolatileProperty("age", age)
                .LogInfo("Test the fluent API!");

            // update the volatile property (persistent properties will remain)
            fluentLogger
                .AddOrUpdateVolatileProperty("age", age+1)
                .LogInfo("Test the fluent API 2!");

            // volatile property not set, will remain empty
            fluentLogger
                .LogInfo("Test the fluent API 3!");

            var logger = new GlobalLogger();
            // Explicitly clear the persistent properties! TODO decide if the clear method should be available from fluen API?
            fluentLogger.ClearPersistentProperties();
            fluentLogger.LogInfo("Test the fluent API 4!");

            logger.StartTimer("Custom");

            // test parameterless ctor
            
            logger.LogTrace("new Employee created (custom parameterless ctor)");

            logger.StopTimer("Custom", Level.Info, "Custom time: {0} ", TimeUnit.Ticks);
            
            logger.StartTimer("Standard");
            
            var test = LogManager.GetCurrentClassLogger();
            test.Log(LogLevel.Trace, "new Employee created (standard)");

            logger.StopTimer("Standard", Level.Info, "Standard time: {0}", TimeUnit.Ticks);

           // Console.WriteLine("Custom time: " + custom.ElapsedMilliseconds + "ms | " + custom.ElapsedTicks + " ticks.");
           // Console.WriteLine("Standard time: " + standard.ElapsedMilliseconds + "ms | " + standard.ElapsedTicks + " ticks.");

           
        }
    }
}
