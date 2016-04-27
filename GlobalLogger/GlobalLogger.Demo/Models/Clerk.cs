namespace GlobalLogger.Demo.Models
{
    public class Clerk : Employee
    {
        public Clerk(string firstName, string lastName, int age, int badgeNumber)
            : base(firstName, lastName, age, badgeNumber)
        {
            Position = "Clerk";
        }
    }
}
