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
            var loginService = new LoginService();
            var vacationService = new VacationService(loginService); 
            var operationLog = new List<string>();

            while (true)
            {
                User user = null;
                while (user == null)
                {
                    Console.Clear();
                    Utility.WriteTitle("Vacation Manager");
                    Utility.WriteMessage("Username: ");
                    string username = Console.ReadLine();
                    string password = Utility.ReadPassword("Password: ");

                    try
                    {
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
                    catch (Exception)
                    {
                        Utility.WriteErrorMessage("An error occurred during login.");
                        if (Console.ReadKey().KeyChar == 'c') return;
                    }
                }

                try
                {
                    MainMenu.Show(user, vacationService, operationLog);
                }
                catch (Exception)
                {
                    Utility.WriteErrorMessage("An error occurred in the main menu.");
                    if (Console.ReadKey().KeyChar == 'c') return;
                }
            }
        }
    }
}
