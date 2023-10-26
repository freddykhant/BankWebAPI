using BankWebAPI.Models;
using System.Data.SQLite;
using System.Reflection;


// THIS IS THE NEW ONE

namespace BankWebAPI.Data
{
    public class DBManager
    {
        private static string connectionString = "Data Source=BankWebService.db;Version=3;";
        //private static string connectionString = "Data Source=mydatabase.db;Version=3;";

        public static bool CreateTables()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {

                    command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS Account (
                            AccountNumber INTEGER PRIMARY KEY,
                            UserNumber INTEGER,
                            Balance REAL,
                            AccountHolderName TEXT
                        )";

                    command.ExecuteNonQuery();
                    // this has to be called AccountTransaction because Transaction is a reserved keyword in SQL lol
                    command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS AccountTransaction (
                            TransactionId INTEGER PRIMARY KEY,
                            AccountNumber INTEGER,
                            Type TEXT,
                            Amount REAL,
                            Timestamp TEXT,
                            FOREIGN KEY(AccountNumber) REFERENCES Account(AccountNumber)
                        )";
                    command.ExecuteNonQuery();



                    // MAKE USER NUMBER INSTEAD OF ACCOUNT NUMBER LOLOL
                    command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS UserProfile (
                            Username TEXT PRIMARY KEY,
                            Email TEXT,
                            Name TEXT,
                            Address TEXT,
                            Phone TEXT,
                            Picture BLOB,
                            Password TEXT,
                            UserNumber TEXT
                        )";
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            return true;
        }

        // Account methods

        public static bool InsertAccount(Account account)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                INSERT INTO Account (AccountNumber, UserNumber, Balance, AccountHolderName)
                VALUES (@AccountNumber, @UserNumber, @Balance, @AccountHolderName)";

                    command.Parameters.AddWithValue("@AccountNumber", account.AccountNumber);
                    
                    command.Parameters.AddWithValue("@Balance", account.Balance);

                    command.Parameters.AddWithValue("@AccountHolderName", account.AccountName);

                    command.Parameters.AddWithValue("@UserNumber", account.UserNumber);

                    int rowsInserted = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsInserted > 0;
                }
            }
        }

        public static bool UpdateAccount(Account account)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                UPDATE Account 
                SET AccountHolderName = @AccountHolderName, Balance = @Balance 
                WHERE AccountNumber = @AccountNumber";

                    command.Parameters.AddWithValue("@AccountNumber", account.AccountNumber);
                    command.Parameters.AddWithValue("@AccountHolderName", account.AccountName);
                    command.Parameters.AddWithValue("@Balance", account.Balance);

                    int rowsUpdated = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsUpdated > 0;
                }
            }
        }

        public static bool DeleteAccount(int accountNumber)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM Account WHERE AccountNumber = @AccountNumber";
                    command.Parameters.AddWithValue("@AccountNumber", accountNumber);

                    int rowsDeleted = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsDeleted > 0;
                }
            }
        }

        public static Account GetAccountByNumber(int accountNumber)
        {
            //accountNumber = accountNumber - 1000;
            Account account = null;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString)) // was like SqliteConnection before???
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM Account WHERE AccountNumber = @AccountNumber";
                    command.Parameters.AddWithValue("@AccountNumber", accountNumber);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            account = new Account
                            {
                                AccountNumber = Convert.ToInt32(reader["AccountNumber"]),
                                UserNumber = Convert.ToInt32(reader["UserNumber"]),
                                AccountName = reader["AccountHolderName"].ToString(),
                                Balance = (decimal)Convert.ToDouble(reader["Balance"])
                            };
                        }
                    }
                }
                connection.Close();
            }
            return account;
        }

        public static List<Account> GetAccountsByUserNumber(int userNumber)
        {
            //accountNumber = accountNumber - 1000;
            List<Account> accounts = new List<Account>();
            //Account account = null;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString)) // was like SqliteConnection before???
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM Account WHERE UserNumber = @UserNumber";
                    command.Parameters.AddWithValue("@UserNumber", userNumber);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Account account = new Account
                            {
                                AccountNumber = Convert.ToInt32(reader["AccountNumber"]),
                                UserNumber = Convert.ToInt32(reader["UserNumber"]),
                                AccountName = reader["AccountHolderName"].ToString(),
                                Balance = (decimal)Convert.ToDouble(reader["Balance"])
                            };
                            accounts.Add(account);
                        }
                    }
                }
                connection.Close();
            }
            return accounts;
        }

        public static List<Account> GetAllAccounts()
        {
            List<Account> accounts = new List<Account>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM Account";
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Account account = new Account
                            {
                                AccountNumber = Convert.ToInt32(reader["AccountNumber"]),
                                UserNumber = Convert.ToInt32(reader["UserNumber"]),
                                AccountName = reader["Username"].ToString(),
                                Balance = (decimal)Convert.ToDouble(reader["Balance"])
                            };
                            accounts.Add(account);
                        }
                    }
                }
                connection.Close();
            }
            return accounts;
        }

        // transaction methods

        public static bool InsertTransaction(Transaction transaction)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        INSERT INTO AccountTransaction (AccountNumber, Type, Amount, Timestamp)
                        VALUES (@AccountNumber, @Type, @Amount, @Timestamp)";

                    command.Parameters.AddWithValue("@AccountNumber", transaction.AccountNumber);
                    command.Parameters.AddWithValue("@Type", transaction.Type);
                    command.Parameters.AddWithValue("@Amount", transaction.Amount);
                    command.Parameters.AddWithValue("@Timestamp", transaction.Timestamp);

                    int rowsInserted = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsInserted > 0;
                }
            }
        }

        public static bool UpdateTransaction(Transaction transaction)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                    UPDATE Transaction 
                    SET AccountNumber = @AccountNumber, Type = @Type, Amount = @Amount, Timestamp = @Timestamp 
                    WHERE TransactionId = @TransactionId";

                    command.Parameters.AddWithValue("@TransactionId", transaction.TransactionId);
                    command.Parameters.AddWithValue("@AccountNumber", transaction.AccountNumber);
                    command.Parameters.AddWithValue("@Type", transaction.Type);
                    command.Parameters.AddWithValue("@Amount", transaction.Amount);
                    command.Parameters.AddWithValue("@Timestamp", transaction.Timestamp);

                    int rowsUpdated = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsUpdated > 0;
                }
            }
        }

        public static bool DeleteTransaction(int transactionId)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM Transaction WHERE TransactionId = @TransactionId";
                    command.Parameters.AddWithValue("@TransactionId", transactionId);

                    int rowsDeleted = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsDeleted > 0;
                }
            }
        }

        public static Transaction GetTransactionById(int id)
        {
            Transaction transaction = null;

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM AccountTransaction WHERE TransactionId = @TransactionId";
                    command.Parameters.AddWithValue("@TransactionId", id);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            transaction = new Transaction
                            {
                                TransactionId = Convert.ToInt32(reader["TransactionId"]),
                                AccountNumber = Convert.ToInt32(reader["AccountNumber"]),
                                Type = (TransactionType)reader["Type"],
                                Amount = (decimal)Convert.ToDouble(reader["Amount"]),
                                Timestamp = DateTime.Parse(reader["Timestamp"].ToString())
                            };
                        }
                    }
                }
                connection.Close();
            }

            return transaction;
        }

        public static List<Transaction> GetTransactionByAccountNumber(int accountNumber)
        {
            List<Transaction> transactions = new List<Transaction>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM AccountTransaction WHERE AccountNumber = @AccountNumber";
                    command.Parameters.AddWithValue("@AccountNumber", accountNumber);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Transaction transaction = new Transaction
                            {
                                TransactionId = Convert.ToInt32(reader["TransactionId"]),
                                AccountNumber = Convert.ToInt32(reader["AccountNumber"]),
                                Type = (TransactionType)Convert.ToInt32(reader["Type"]),
                                Amount = (decimal)Convert.ToDouble(reader["Amount"]),
                                Timestamp = DateTime.Parse(reader["Timestamp"].ToString())
                            };
                            transactions.Add(transaction);
                        }
                    }
                }
                connection.Close();
            }

            return transactions;
        }

        public static List<Transaction> GetAllTransactions()
        {
            List<Transaction> transactions = new List<Transaction>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM Transaction";
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Transaction transaction = new Transaction
                            {
                                TransactionId = Convert.ToInt32(reader["TransactionId"]),
                                AccountNumber = Convert.ToInt32(reader["AccountNumber"]),
                                Type = (TransactionType)reader["Type"],
                                Amount = (decimal)Convert.ToDouble(reader["Amount"]),
                                Timestamp = DateTime.Parse(reader["Timestamp"].ToString())
                            };
                            transactions.Add(transaction);
                        }
                    }
                }
                connection.Close();
            }
            return transactions;
        }


        // user profile

        public static bool InsertUserProfile(UserProfile userProfile)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        INSERT INTO UserProfile (Username, Email, Name, Address, Phone, Picture, Password, UserNumber)
                        VALUES (@Username, @Email, @Name, @Address, @Phone, @Picture, @Password, @UserNumber)";

                    command.Parameters.AddWithValue("@Username", userProfile.Username);
                    command.Parameters.AddWithValue("@Email", userProfile.Email);
                    command.Parameters.AddWithValue("@Name", userProfile.Name);
                    command.Parameters.AddWithValue("@Address", userProfile.Address);
                    command.Parameters.AddWithValue("@Phone", userProfile.Phone);
                    command.Parameters.AddWithValue("@Picture", userProfile.Picture);
                    command.Parameters.AddWithValue("@Password", userProfile.Password);
                    command.Parameters.AddWithValue("@UserNumber", userProfile.UserNumber);

                    int rowsInserted = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsInserted > 0;
                }
            }
        }

        public static bool UpdateUserProfile(UserProfile userProfile)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                    UPDATE UserProfile 
                    SET Email = @Email, Name = @Name, Address = @Address, Phone = @Phone, Picture = @Picture, Password = @Password, UserNumber = @UserNumber
                    WHERE UserNumber = @UserNumber";

                    command.Parameters.AddWithValue("@Username", userProfile.Username);
                    command.Parameters.AddWithValue("@Email", userProfile.Email);
                    command.Parameters.AddWithValue("@Name", userProfile.Name);
                    command.Parameters.AddWithValue("@Address", userProfile.Address);
                    command.Parameters.AddWithValue("@Phone", userProfile.Phone);
                    command.Parameters.AddWithValue("@Picture", userProfile.Picture);
                    command.Parameters.AddWithValue("@Password", userProfile.Password);
                    command.Parameters.AddWithValue("@UserNumber", userProfile.UserNumber);

                    int rowsUpdated = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsUpdated > 0;
                }
            }
        }

        public static bool DeleteUserProfile(string username)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM UserProfile WHERE Username = @Username";
                    command.Parameters.AddWithValue("@Username", username);

                    int rowsDeleted = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsDeleted > 0;
                }
            }
        }

        public static UserProfile GetUserProfileById(int id)
        {
            UserProfile userProfile = null;

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM UserProfile WHERE UserNumber = @UserNumber";
                    command.Parameters.AddWithValue("@UserNumber", id);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userProfile = new UserProfile
                            {
                                Username = reader["Username"].ToString(),
                                Name = reader["Name"].ToString(),
                                Email = reader["Email"].ToString(),
                                Address = reader["Address"].ToString(),
                                Phone = reader["Phone"].ToString(),
                                // commenting out the picture for now 
                                //Picture = (byte[])reader["Picture"],
                                Password = reader["Password"].ToString(),
                                UserNumber = reader["UserNumber"].ToString()
                            };
                        }
                    }
                }
                connection.Close();
            }

            return userProfile;
        }

        public static UserProfile GetUserProfileByEmail(string email)
        {
            UserProfile userProfile = null;

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM UserProfile WHERE Email = @Email";
                    command.Parameters.AddWithValue("@Email", email);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userProfile = new UserProfile
                            {
                                Username = reader["Username"].ToString(),
                                Name = reader["Name"].ToString(),
                                Email = reader["Email"].ToString(),
                                Address = reader["Address"].ToString(),
                                Phone = reader["Phone"].ToString(),
                                // commenting out the picture for now 
                                //Picture = (byte[])reader["Picture"],
                                Password = reader["Password"].ToString(),
                                UserNumber = reader["UserNumber"].ToString()
                            };
                        }
                    }
                }
                connection.Close();
            }

            return userProfile;
        }



        public static List<UserProfile> GetAllUserProfiles()
        {
            List<UserProfile> userProfiles = new List<UserProfile>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM UserProfile";
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UserProfile userProfile = new UserProfile
                            {
                                Username = reader["Username"].ToString(),
                                Email = reader["Email"].ToString(),
                                Name = reader["Name"].ToString(),
                                Address = reader["Address"].ToString(),
                                Phone = reader["Phone"].ToString(),
                                Picture = (byte[])reader["Picture"],
                                Password = reader["Password"].ToString(),
                                UserNumber = reader["UserNumber"].ToString()
                            };
                            userProfiles.Add(userProfile);
                        }
                    }
                }
                connection.Close();
            }
            return userProfiles;
        }

        public static void addAdmin()
        {
            UserProfile admin = new UserProfile();

            admin.Email = "admin";
            admin.Password = "password";
            admin.Username = "admin";
            admin.UserNumber = "9999";

            DBManager.InsertUserProfile(admin);
        }

        public static void deleteDB()
        {
            // if you're having problems make sure to check that you don't have the db open
            string filePath = @"C:\Users\fred\Desktop\BankWebAPI\BankWebAPI\BankWebService.db";

            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                    Console.WriteLine("File deleted successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting file: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("File does not exist.");
            }
        }

        public static void DBInitialize()
        {
            if (CreateTables())
            {
            }
        }
    }
}
