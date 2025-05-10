using D00_Utility;
using VacationCore.Entities;
using VacationCore.Services;

namespace VacationApp.Views
{
    public static class LoginView
    {

        public static User ShowLogin(LoginService loginService, string username, string password)
        {
            var user = loginService.Authenticate(username, password);
            return user;
        }
    }
}