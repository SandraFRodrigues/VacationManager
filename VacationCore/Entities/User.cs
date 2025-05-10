using System.Collections.Generic;
using System;

namespace VacationCore.Entities
{
    // Esta classe abstrata representa um utilizador genérico (pode ser Admin ou Colaborador)
    public abstract class User
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public abstract bool IsAdmin { get; }  // Removido o isAdmin = false

        protected User(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public abstract List<string> GetMenuOptions();
    }
}