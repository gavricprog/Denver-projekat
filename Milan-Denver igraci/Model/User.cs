using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milan_Denver_igraci.Model
{
    public enum UserRole
    {
        Visitor,
        Admin
    }

    public class User
    {

        public string Username { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }

        public User()
        {
            Username = string.Empty;
            Password = string.Empty;
            Role = UserRole.Visitor;
        }
    }

}
