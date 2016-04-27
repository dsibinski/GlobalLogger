namespace GlobalLogger.Demo.Models
{
    public class Accountant : Employee
    {
        public Accountant(string firstName, string lastName, int age, int badgeNumber)
            : base(firstName, lastName, age, badgeNumber)
        {
            Position = "Accountant";
        }
    }
}
