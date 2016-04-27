using System;
using GlobalLogger.Demo.Models;

namespace GlobalLogger.Demo
{
    public enum EmployeeType
    {
        Cashier,
        Clerk,
        Accountant,
        StoreManager,
        Random
    }

    public static class EmployeeGenerator
    {
        private static readonly string[] FirstNames =
        {
            "John",
            "Vincent",
            "Emily",
            "Hector",
            "Christian",
            "Henry",
            "Lucy",
            "Erin",
            "Hannah",
            "Jack",
            "David",
            "Allison"
        };

        private static readonly string[] LastNames =
        {
            "Jones",
            "Williams",
            "Taylor",
            "Brown",
            "Davies",
            "Evans",
            "Wilson",
            "Thomas",
            "Johnson",
            "Roberts",
            "Robinson",
            "Thompson",
            "Wright",
            "Walker",
            "White",
            "Edwards",
            "Hughes",
            "Green",
            "Hall",
            "Lewis",
            "Harris",
            "Clarke",
            "Patel"
        };

        public static Employee Generate(DateTime time, EmployeeType type)
        {
            var randomizer = new Random(time.Second + time.Millisecond);

            // generate employee's data
            var firstName = GetRandomFirstName(randomizer);
            var lastName = GetRandomLastName(randomizer);
            var age = randomizer.Next(18, 65);
            var badgeNumber = randomizer.Next(100000, 999999);

            if (type == EmployeeType.Cashier)
                return new Cashier(firstName, lastName, age, badgeNumber);

            if (type == EmployeeType.Clerk)
                return new Clerk(firstName, lastName, age, badgeNumber);

            if (type == EmployeeType.Accountant)
                return new Accountant(firstName, lastName, age, badgeNumber);

            if (type == EmployeeType.StoreManager)
                return new StoreManager(firstName, lastName, age, badgeNumber);

            else // create a random employee
            {
                var number = randomizer.Next(1, 13);

                if (number == 1)
                    return new StoreManager(firstName, lastName, age, badgeNumber);

                if (number >= 2 && number <= 3)
                    return new Accountant(firstName, lastName, age, badgeNumber);

                if (number >= 4 && number <= 6)
                    return new Clerk(firstName, lastName, age, badgeNumber);

                return new Cashier(firstName, lastName, age, badgeNumber);
            }
        }

        private static string GetRandomFirstName(Random randomizer)
        {
            return FirstNames[randomizer.Next(0, FirstNames.Length)];
        }

        private static string GetRandomLastName(Random randomizer)
        {
            return LastNames[randomizer.Next(0, LastNames.Length)];
        }
    }
}
