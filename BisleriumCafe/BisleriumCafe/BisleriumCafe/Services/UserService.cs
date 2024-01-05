using BisleriumCafe.Enums;
using BisleriumCafe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BisleriumCafe.Services
{
    public class UserService
    {
        public static List<User> GetAllUser()
        {
            string userFilePath = Utils.GetUserFilePath();
            if (File.Exists(userFilePath))
            {
                var json = File.ReadAllText(userFilePath);
                return JsonSerializer.Deserialize<List<User>>(json);
            }

            return new List<User>();
        }

        public static void SaveAllUser(List<User> Users)
        {
            string UserFilePath = Utils.GetUserFilePath();
            string appDirectoryFilePath = Utils.GetAppDirectoryPath();

            if (!Directory.Exists(appDirectoryFilePath))
                Directory.CreateDirectory(appDirectoryFilePath);

            var json = JsonSerializer.Serialize(Users);
            File.WriteAllText(UserFilePath, json);
        }

        public static List<User> CreateUser(string password, Role role)
        {
            List<User> users = GetAllUser();

            users.Add(
                new User
                {
                    Password = Utils.HashPassword(password),
                    Role = role,
                }
            );
            SaveAllUser( users );
            return users;
        }
    }
}
