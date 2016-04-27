using System;

namespace GlobalLogger.Demo
{
    public static class RandomStringGenerator
    {
        static public string  GenerateRandomString()
        {
            string myString = "";
            var myRandom = new Random();

            var stringLenght = myRandom.Next(1, 10);

            for (int i = 0; i < stringLenght; i++)
            {
                var randomCharacter = Convert.ToChar(myRandom.Next(1, 100));

                myString += randomCharacter;
            }

            return myString;
        }
    }
}
