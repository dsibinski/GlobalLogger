using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;

namespace GlobalLogger.Tests.UnitTests
{
    public class BaseClass
    {
        public LogHelper Logger { get; set; }
    }

    public class DerivedClass : BaseClass
    {}

    [TestFixture]
    public class GlobalLoggerTests
    {
        [Test]
        public void TestDefaultConstructor_LoggerName_OK()
        {
            var type = new BaseClass();
            type.Logger = new LogHelper();

            type.Logger.Name.Should().Be("GlobalLogger.Tests.UnitTests.GlobalLoggerTests", "because we are using the default constructor (calling class)");
        }

        [Test]
        public void TestTypeConstructorBaseClass__LoggerName_OK()
        {
            var type = new BaseClass();
            type.Logger = new LogHelper(type);

            type.Logger.Name.Should().Be("GlobalLogger.Tests.UnitTests.BaseClass", "because we are using the type constructor on the base class");
        }

        [Test]
        public void TestTypeConstructorDerivedClass__LoggerName_OK()
        {
            var type = new DerivedClass();
            type.Logger = new LogHelper(type);

            type.Logger.Name.Should().Be("GlobalLogger.Tests.UnitTests.DerivedClass", "because we are using the type constructor on the derived class");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestTypeConstructor_ArgumentNullException()
        {
            string loggerName = null;
            BaseClass type = null;
            type.Logger = new LogHelper(type);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestStringConstructor_ArgumentNullException()
        {
            string loggerName = null;
            var type = new BaseClass();
            type.Logger = new LogHelper(loggerName);
        }

        [Test]
        public void TestVolatileProperty_ClearAfterLog_OK()
        {
            var type = new BaseClass();
            type.Logger = new LogHelper(type);

            type.Logger.AddOrUpdateVolatileProperty("age", 10);

            var volatileProperties = typeof(LogHelper).GetField("_volatileProperties", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(type.Logger) as Dictionary<string, Object>;

            volatileProperties.Should().NotBeNull("because volatile properties must always be initialized");
            volatileProperties.Count.Should().Be(1, "because one volatile property was added");
            volatileProperties.Keys.FirstOrDefault().Should().Be("age", "because the 'age' property was added");
            volatileProperties.Values.FirstOrDefault().Should().Be(10, "because the 'age' is '10'");
            
            type.Logger.LogError("test");

            volatileProperties.Count.Should().Be(0, "because the message was logged and the volatile properties were then implicitly cleared");
        }

        [Test]
        public void TestVolatileProperty_ExplicitClear_OK()
        {
            var type = new BaseClass();
            type.Logger = new LogHelper(type);

            type.Logger.AddOrUpdateVolatileProperty("age", 10);

            var volatileProperties = typeof(LogHelper).GetField("_volatileProperties", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(type.Logger) as Dictionary<string, Object>;

            volatileProperties.Should().NotBeNull("because volatile properties must always be initialized");
            volatileProperties.Count.Should().Be(1, "because one volatile property was added");
            volatileProperties.Keys.FirstOrDefault().Should().Be("age", "because the 'age' property was added");
            volatileProperties.Values.FirstOrDefault().Should().Be(10, "because the 'age' is '10'");

            type.Logger.ClearVolatileProperties();

            volatileProperties.Count.Should().Be(0, "because volatile properties were explicitly cleared");
        }

        [Test]
        public void TestPersistentProperty_PersistentAfterLog_OK()
        {
            var type = new BaseClass();
            type.Logger = new LogHelper(type);

            type.Logger.AddOrUpdatePersistentProperty("age", 10);

            var persistentProperties = typeof(LogHelper).GetField("_persistentProperties", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(type.Logger) as Dictionary<string, Object>;

            persistentProperties.Should().NotBeNull("because volatile properties must always be initialized");
            persistentProperties.Count.Should().Be(1, "because one volatile property was added");
            persistentProperties.Keys.FirstOrDefault().Should().Be("age", "because the 'age' property was added");
            persistentProperties.Values.FirstOrDefault().Should().Be(10, "because the 'age' is '10'");

            type.Logger.LogError("test");

            persistentProperties.Count.Should().Be(1, "because the message was logged and the persistent properties are not implicitly cleared");
        }

        [Test]
        public void TestPersistentProperty_ExplicitClear_OK()
        {
            var type = new BaseClass();
            type.Logger = new LogHelper(type);

            type.Logger.AddOrUpdatePersistentProperty("age", 10);

            var persistentProperties = typeof(LogHelper).GetField("_persistentProperties", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(type.Logger) as Dictionary<string, Object>;

            persistentProperties.Should().NotBeNull("because persistent properties must always be initialized");
            persistentProperties.Count.Should().Be(1, "because one persistent property was added");
            persistentProperties.Keys.FirstOrDefault().Should().Be("age", "because the 'age' property was added");
            persistentProperties.Values.FirstOrDefault().Should().Be(10, "because the 'age' is '10'");

            type.Logger.ClearPersistentProperties();

            persistentProperties.Count.Should().Be(0, "because persistent properties were explicitly cleared");
        }

        [Test]
        public void TestDefaultConstructor_Init_OK()
        {
            var type = new BaseClass();
            type.Logger = new LogHelper();

            CheckIfPropertiesProperlyInitialized(type);
        }

        [Test]
        public void TestTypeConstructor_Init_OK()
        {
            var type = new DerivedClass();
            type.Logger = new LogHelper(type);

            CheckIfPropertiesProperlyInitialized(type);
        }

        [Test]
        public void TestStringConstructor_Init_OK()
        {
            var loggerName = "myCustomLoggerName";
            var type = new DerivedClass();
            type.Logger = new LogHelper(loggerName);

            CheckIfPropertiesProperlyInitialized(type);
        }

        private static void CheckIfPropertiesProperlyInitialized(BaseClass type)
        {
            var persistentProperties = typeof(LogHelper).GetField("_persistentProperties", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(type.Logger);
            var volatileProperties = typeof(LogHelper).GetField("_volatileProperties", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(type.Logger);

            persistentProperties.Should().NotBe(null, "because InitializePropertiesContainers must be called from every constructor");
            volatileProperties.Should().NotBe(null, "because InitializePropertiesContainers must be called from every constructor");

            persistentProperties.Should().BeOfType(typeof (Dictionary<string, object>), "because persistentProperties is a Dictionary<string, Object>");
            volatileProperties.Should().BeOfType(typeof (Dictionary<string, object>), "because volatileProperties is a Dictionary<string, Object>");

            var castPersistentProperties = persistentProperties as Dictionary<string, Object>;
            var castVolatileProperties = volatileProperties as Dictionary<string, Object>;

            castPersistentProperties.Count.Should().Be(0, "because persistentProperties should be empty right after initialization");
            castVolatileProperties.Count.Should().Be(0, "because persistentProperties should be empty right after initialization");
        }
    }
}
