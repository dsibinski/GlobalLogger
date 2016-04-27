namespace GlobalLogger.Demo.Models
{
    public class Cashier : Employee
    {
        public Cashier(string firstName, string lastName, int age, int badgeNumber)
            : base(firstName, lastName, age, badgeNumber)
        {
            Position = "Cashier";
        }
    }
}
