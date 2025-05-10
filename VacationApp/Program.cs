using D00_Utility;
using System;
using VacationCore.Entities;
using VacationCore.Services;
using VacationApp.Views;
using System.Collections.Generic;

namespace VacationApp
{
    class Program
    {
        static void Main()
        {

            var vacationService = new VacationService();  // Serviço de férias
            var loginService = new LoginService();  // Serviço de autenticação
            var operationLog = new List<string>();  // Histórico de operações

            while (true)
            {
                User user = null;
                while (user == null)
                {
                    Console.Clear();
                    Utility.WriteTitle("Vacation manager");

                    // Realiza o login com o LoginService
                    Utility.WriteMessage("Username: ");
                    string username = Console.ReadLine();
                    Utility.WriteMessage("Password: ");
                    string password = Console.ReadLine();

                    // Usa o LoginService para autenticar
                    user = LoginView.ShowLogin(loginService, username, password);

                    if (user == null)
                    {
                        Utility.WriteErrorMessage("Invalid credentials. Try again or press 'c' to cancel.");
                        if (Console.ReadKey().KeyChar == 'c') return;
                    }
                    else
                    {
                        Utility.ShowGreeting();
                    }
                }

                // Exibe o menu após o login bem-sucedido, passando o operationLog também
                MainMenu.Show(user, vacationService, operationLog);
            }
        }
    }
}