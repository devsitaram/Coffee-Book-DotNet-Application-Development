using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BisleriumCafe.Data.Enums;
using BisleriumCafe.Data.Models;

namespace BisleriumCafe.Data.Services
{
    public class UserServices
    {
        public const Role LoginRole = Role.Admin;

        public const string Password = "Admin";
        public static User CurrentUser { get; set; }

        // get all user
        public static List<User> GetAllUser()
        {
            try
            {
                string appUsersFilePath = Utils.GetUsersFilePath();
                if (!File.Exists(appUsersFilePath))
                {
                    return new List<User>();
                }
                var json = File.ReadAllText(appUsersFilePath);
                return JsonSerializer.Deserialize<List<User>>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<User>();
            }

            
        }

        // create the new user
        public static List<User> CreateNewUser(string password, Role role)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<User>();
            }
        }

        // default register the both user
        public static void SeedUsers()
        {
            var users = GetAllUser().FirstOrDefault(x => x.Role == Role.Admin);
            if (users == null)
            {
                CreateNewUser("Admin", Role.Admin);
                CreateNewUser("Staff", Role.Staff);
            }
        }

        // check the login user vali or not
        public static User Login(Role role, string password)
        {
            try
            {
                var loginErrorMessage = "Invalid role or password.";
                List<User> users = GetAllUser();
                User user = users.FirstOrDefault(x => x.Role == role);
                if (user == null)
                {
                    throw new Exception(loginErrorMessage);
                } 
                else
                {
                    //checking if the password is valid or not using password hash 
                    bool passwordIsValid = Utils.VerifyHash(password, user.PasswordHash);

                    if (!passwordIsValid)
                    {
                        throw new Exception(loginErrorMessage);
                    }
                    CurrentUser = user;
                    return user;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new User();
            }
        }

        // password change
        public static string ChangePassword(Guid Id, string newPassword, string confirmPassword)
        {
            try
            {
                if (newPassword != confirmPassword)
                {
                    return "New password and confirm password do not match!";
                }
                else
                {
                    List<User> users = GetAllUser();
                    // Find the user based on the specified role
                    User existingPassword = users.FirstOrDefault(x => x.Id == Id);
                    if (existingPassword == null)
                    {
                        return "Invalid user role!";
                    }
                    else
                    {
                        // Proceed to change the password
                        existingPassword.PasswordHash = Utils.HashSecret(newPassword);
                        SaveAll(users);
                        return "success";
                    }
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        // save user in app directory path
        private static void SaveAll(List<User> users)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
