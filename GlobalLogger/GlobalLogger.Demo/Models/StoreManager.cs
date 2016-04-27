namespace GlobalLogger.Demo.Models
{
    public class StoreManager : Employee
    {
        public StoreManager(string firstName, string lastName, int age, int badgeNumber)
            : base(firstName, lastName, age, badgeNumber)
        {
            Position = "Store Manager";
        }
    }
}
