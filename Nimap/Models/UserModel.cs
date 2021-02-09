using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nimap.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public UserRole Role { get; set; }
    }

    public enum UserRole
    {
        Admin = 1,
        Supervisor = 2,
        Customer = 3
    }
}