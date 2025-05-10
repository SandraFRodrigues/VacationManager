using D00_Utility;
using System;
using VacationCore.Entities;
using VacationCore.Services;

namespace VacationApp.Views
{
    public static class LoginView
    {
        public static User ShowLogin(LoginService loginService, string username, string password)
        {
            // Chama o LoginService para autenticar o utilizador
            var user = loginService.Authenticate(username, password);
            return user;  // Retorna o utilizador autenticado ou null se não encontrar
        }
    }
}