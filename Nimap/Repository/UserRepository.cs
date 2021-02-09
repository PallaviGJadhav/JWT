using Nimap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nimap.Repository
{
    public class UserRepository
    {
        public List<UserModel> userList;

        public UserRepository()
        {
            userList = new List<UserModel>();
            userList.Add(new UserModel() { UserId = 1, UserName = "admin@nimap.com", Password = "123", Role = UserRole.Admin });
            userList.Add(new UserModel() { UserId = 2, UserName = "supervisor@nimap.com", Password = "123", Role = UserRole.Supervisor });
            userList.Add(new UserModel() { UserId = 3, UserName = "customer@nimap.com", Password = "123", Role = UserRole.Customer });
        }

        public UserModel GetUser(string userName)
        {
            try
            {
                return userList.First(u => u.UserName == userName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}