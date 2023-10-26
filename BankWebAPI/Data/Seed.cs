using BankWebAPI.Models;

namespace BankWebAPI.Data
{
    public class Seed
    {
        public static void SeedData()
        {
            Random random = new Random();

            // user profiles
            for (int i = 0; i < 100; i++)
            {
                UserProfile user = new UserProfile
                {
                    Username = $"user{i}",
                    Email = $"user{i}@example.com",
                    Name = $"User {i}",
                    Address = $"Address {i}",
                    Phone = $"123-456-78{i}",
                    Password = $"password{i}",
                    UserNumber = (1000 + i).ToString(),
                };
                DBManager.InsertUserProfile(user);
            }

            // accounts

            for (int i = 0; i < 100; i++)
            {
                Account account = new Account
                {
                    AccountNumber = i,
                    UserNumber = 1000 + i,
                    Balance = random.Next(1000, 10000),
                    AccountName = $"user{i}"
                };
                DBManager.InsertAccount(account);
            }
            for (int i = 0; i < 100; i++)
            {
                Account account = new Account
                {
                    AccountNumber = 100 + i,
                    UserNumber = 1000 + i,
                    Balance = random.Next(1000, 10000),
                    AccountName = $"user{i}"
                };
                DBManager.InsertAccount(account);
            }

            // transactions

            for (int i = 0; i < 300; i++)
            {
                int month = random.Next(1, 13);
                int day;
                switch (month)
                {
                    case 1:
                    case 3:
                    case 5:
                    case 7:
                    case 8:
                    case 10:
                    case 12:
                        day = random.Next(1, 32);
                        break;
                    case 2:
                        day = random.Next(1, 29); 
                        break;
                    default:
                        day = random.Next(1, 31);
                        break;
                }

                int hour = random.Next(0, 24);
                int minute = random.Next(0, 60);
                int second = random.Next(0, 60);

                Transaction transaction = new Transaction
                {
                    AccountNumber = random.Next(0, 199),
                    Type = (TransactionType)random.Next(0, 2),
                    Amount = random.Next(10, 500),
                    Timestamp = new DateTime(2023, month, day, hour, minute, second)
                };

                DBManager.InsertTransaction(transaction);
            }

        }
    }
}
