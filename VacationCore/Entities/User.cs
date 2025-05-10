using System.Collections.Generic;
using System;

namespace VacationCore.Entities
{
    public abstract class User
    {
        public string Username { get; private set; }
        public string Password { get; private set; }

        protected User(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public abstract bool IsAdmin { get; }

        public abstract List<string> GetMenuOptions();
    }
}