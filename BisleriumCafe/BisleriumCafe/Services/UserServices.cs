using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BisleriumCafe.Enums;
using BisleriumCafe.Models;

namespace BisleriumCafe.Services
{
    public class UserServices
    {
        public const string SeedUsername = "admin";
        public const string SeedPassword = "admin";

 

        public static List<User> GetAllUser()
        {
            string appUsersFilePath = Utils.GetUsersFilePath();
            if (!File.Exists(appUsersFilePath))
            {
                return new List<User>();
            }

            var json = File.ReadAllText(appUsersFilePath);

            return JsonSerializer.Deserialize<List<User>>(json);
        }

        // create the user
        public static List<User> CreateNewUser(Guid userId, string password, Role role)
        {
            List<User> users = GetAllUser();
            bool usernameExists = users.Any(x => x.Role == role);

            if (usernameExists)
            {
                throw new Exception("Users already exists.");
            }

            users.Add(
                new User
                {
                    PasswordHash = Utils.HashSecret(password),
                    Role = role,
                }
            );
            SaveAll(users);
            return users;
        }

        public static void SeedUsers()
        {
            var users = GetAllUser().FirstOrDefault(x => x.Role == Role.Admin);

            if (users == null)
            {
                CreateNewUser(Guid.Empty, SeedPassword, Role.Admin);
            }
        }

        public static User GetById(Guid id)
        {
            List<User> users = GetAllUser();
            return users.FirstOrDefault(x => x.Id == id);
        }


        // delete user
        public static List<User> DeleteUser(Guid id)
        {
            List<User> users = GetAllUser();
            User user = users.FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            users.Remove(user);
            SaveAll(users);
            return users;
        }



        public static User Login(Role role, string password)
        {
            var loginErrorMessage = "Invalid rolr or password.";
            List<User> users = GetAllUser();
            User user = users.FirstOrDefault(x => x.Role == role);

            if (user == null)
            {
                throw new Exception(loginErrorMessage);
            }

            bool passwordIsValid = Utils.VerifyHash(password, user.PasswordHash);

            if (!passwordIsValid)
            {
                throw new Exception(loginErrorMessage);
            }

            return user;
        }

        public static User ChangePassword(Guid id, string currentPassword, string newPassword)
        {
            if (currentPassword == newPassword)
            {
                throw new Exception("New password must be different from current password.");
            }

            List<User> users = GetAllUser();
            User user = users.FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            bool passwordIsValid = Utils.VerifyHash(currentPassword, user.PasswordHash);

            if (!passwordIsValid)
            {
                throw new Exception("Incorrect current password.");
            }

            user.PasswordHash = Utils.HashSecret(newPassword);
            SaveAll(users);
            return user;
        }

        // save user in app directory path
        private static void SaveAll(List<User> users)
        {
            string appDataDirectoryPath = Utils.GetAppDirectoryPath();
            string appUsersFilePath = Utils.GetUsersFilePath();

            if (!Directory.Exists(appDataDirectoryPath))
            {
                Directory.CreateDirectory(appDataDirectoryPath);
            }

            var json = JsonSerializer.Serialize(users);
            File.WriteAllText(appUsersFilePath, json);
        }
    }
}
