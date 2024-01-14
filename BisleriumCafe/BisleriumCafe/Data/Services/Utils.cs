using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafe.Data.Services
{
    public static class Utils
    {
        private const char _segmentDelimiter = ':';

        // get the system file pathe where create the new local document file
        public static string GetAppDirectoryPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Bislerium-Cafe-Data");
        }

        // get user file path
        public static string GetUsersFilePath()
        {
            return Path.Combine(GetAppDirectoryPath(), "users.json");
        }

        // get the customer file path
		public static string GetCustomersFilePath()
		{
			return Path.Combine(GetAppDirectoryPath(), "customers.json");
		}

        // customer order file path
		public static string GetOrderFilePath()
        {
            return Path.Combine(GetAppDirectoryPath(), "orders.json");
        }

        // total purchase coffee file path 
        public static string GetRevenueFilePath()
        {
            return Path.Combine(GetAppDirectoryPath(), "sale_transactions.json");
        }

        // add-ins coffee flavor path directory
        public static string GetAddInFilePath()
        {
            return Path.Combine(GetAppDirectoryPath(), "add_ins_flavors.json");
        }

        // user file path directory
        public static string GetUserFilePath()
        {
            return Path.Combine(GetAppDirectoryPath(), "users.json");
        }

        // coffee file path directory
        public static string GetCoffeeFilePath()
        {
            return Path.Combine(GetAppDirectoryPath(), "coffee.json");
        }


        // password convert the hash format
        public static string HashSecret(string input)
        {
            var saltSize = 16;
            var iterations = 100_000;
            var keySize = 32;
            HashAlgorithmName algorithm = HashAlgorithmName.SHA256;
            byte[] salt = RandomNumberGenerator.GetBytes(saltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(input, salt, iterations, algorithm, keySize);

            return string.Join(
                _segmentDelimiter,
                Convert.ToHexString(hash),
                Convert.ToHexString(salt),
                iterations,
                algorithm
            );
        }


        // password hash form is check the success or not
        public static bool VerifyHash(string input, string hashString)
        {
            string[] segments = hashString.Split(_segmentDelimiter);
            byte[] hash = Convert.FromHexString(segments[0]);
            byte[] salt = Convert.FromHexString(segments[1]);
            int iterations = int.Parse(segments[2]);
            HashAlgorithmName algorithm = new(segments[3]);
            byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(
                input,
                salt,
                iterations,
                algorithm,
                hash.Length
            );
            return CryptographicOperations.FixedTimeEquals(inputHash, hash);
        }
    }
}