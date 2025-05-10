using System.Collections.Generic;
using VacationCore.Entities;
using System.Linq;

namespace VacationCore.Services
{
    public class LoginService
    {
        private readonly List<User> _users;
        public LoginService()
        {
            _users = new List<User>
            {
                new AdminUser("admin", "admin"),
                new EmployeeUser("colaborador01", "123"),
                new EmployeeUser("colaborador02", "123")
            };
        }

        public User Authenticate(string username, string password)
        {
            return _users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }
        public List<User> GetAllUsers()
        {
            return _users;
        }

    }
}