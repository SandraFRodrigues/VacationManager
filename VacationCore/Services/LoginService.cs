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
            // Definição dos utilizadores como "admin" e "colaborador"
            _users = new List<User>
            {
                new AdminUser("admin", "admin"),
                new EmployeeUser("colaborador01", "123"),
                new EmployeeUser("colaborador02", "123")
            };
        }

        // Método para autenticar o utilizador
        public User Authenticate(string username, string password)
        {
            // Retorna o primeiro utilizador que tem o username e password válidos
            return _users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }
    }
}