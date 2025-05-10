using System;
using VacationCore.Entities;
using VacationCore.Services;
using D00_Utility;
using System.Collections.Generic;

namespace VacationApp
{
    public static class MainMenu
    {
        #region Menu
       
        public static void Show(User user, VacationService vacationService, List<string> operationLog)
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Utility.WriteTitle("Vacation Manager");

                Utility.WriteMessage($"\n\nWelcome, {user.Username}!\n");
                Utility.ShowGreeting();
                Utility.WriteMessage("\n");

                int used = vacationService.GetUsedVacationDays(user.Username);
                int remaining = vacationService.GetRemainingVacationDays(user.Username);

                Utility.WriteMessage($"\n\nVacation Summary for {user.Username}:\nUsed: {used} day(s)\nRemaining: {remaining} day(s)\n\n");

                foreach (string option in user.GetMenuOptions())
                {
                    Utility.WriteMessage(option);
                }

                Utility.WriteMessageWithColor("\nSelect option: ", ConsoleColor.Cyan);
                string input = Console.ReadLine()?.Trim();

                switch (input)
                {
                    case "1":
                        RegisterVacation(user, vacationService, operationLog);
                        break;
                    case "2":
                        ListVacations(user, vacationService);
                        break;
                    case "3":
                        UpdateVacation(user, vacationService, operationLog);
                        break;
                    case "4":
                        if (user.IsAdmin)
                            RegisterEmployeeVacation(vacationService, operationLog);
                        else
                            Utility.WriteErrorMessage("Invalid option.");
                        break;
                    case "5":
                        if (user.IsAdmin)
                            UpdateEmployeeVacation(vacationService, operationLog);
                        else
                            Utility.WriteErrorMessage("Invalid option.");
                        break;
                    case "9":
                        return;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Utility.WriteErrorMessage("Invalid option.");
                        break;
                }
            }
        }
        #endregion

        #region Method to register vacation

        private static void RegisterVacation(User user, VacationService vacationService, List<string> operationLog)
        {
            Utility.WriteTitle("Register Vacation");

            bool addMore = false;
            do
            {
                var period = GetVacationPeriodFromUser();

                try
                {
                    if (vacationService.IsOverlapping(user.Username, period.StartDate, period.EndDate))
                    {
                        Utility.WriteErrorMessage("\nVacation period overlaps with existing vacation.");
                        continue;
                    }

                    if (vacationService.GetUsedVacationDays(user.Username) + vacationService.CountWeekdays(period.StartDate, period.EndDate) > 22)
                    {
                        Utility.WriteErrorMessage("\nExceeded the maximum of 22 vacation days.");
                        continue;
                    }

                    vacationService.RegisterVacation(user.Username, period);

                    operationLog.Add($"{user.Username} registered vacation: {period.StartDate:yyyy-MM-dd} to {period.EndDate:yyyy-MM-dd}");
                    Utility.WriteMessageWithColor("Vacation registered successfully.\n", ConsoleColor.Green);

                    ShowVacationSummary(user, vacationService);
                    addMore = AskToAddMoreVacations();
                }
                catch (Exception ex)
                {
                    Utility.WriteErrorMessage($"Error: {ex.Message}");
                    addMore = false;
                }

            } while (addMore);
        }
        #endregion

        #region Method to list vacations
        private static void ListVacations(User user, VacationService vacationService)
        {
            Utility.WriteTitle(user.IsAdmin ? "List Own Vacations" : "List Vacations");

            var vacations = vacationService.GetVacations(user.Username);
            if (vacations == null || vacations.Count == 0)
            {
                Utility.WriteErrorMessage("No vacations registered.");
            }
            else
            {
                foreach (var v in vacations)
                {
                    Utility.WriteMessage($"ID: {v.Id} - {v.StartDate:yyyy-MM-dd} to {v.EndDate:yyyy-MM-dd}");
                }
            }

            Utility.WriteMessage("\nPress any key to return to the menu...");
            Console.ReadKey();
        }
        #endregion

        #region Method to Update Vacation
        private static void UpdateVacation(User user, VacationService vacationService, List<string> operationLog)
        {
            Utility.WriteTitle("Update Vacation");

            var vacations = vacationService.GetVacations(user.Username);
            if (vacations == null || vacations.Count == 0)
            {
                Utility.WriteErrorMessage("No vacations registered.");
                return;
            }

            foreach (var v in vacations)
            {
                Utility.WriteMessage($"ID: {v.Id} - {v.StartDate:yyyy-MM-dd} to {v.EndDate:yyyy-MM-dd}");
            }

            Utility.WriteMessage("\nEnter the vacation ID to update:");
            string input = Console.ReadLine();

            if (!int.TryParse(input, out int vacationId))
            {
                Utility.WriteErrorMessage("Invalid ID.");
                return;
            }

            var vacation = vacationService.GetVacationById(vacationId, user.Username);
            if (vacation == null)
            {
                Utility.WriteErrorMessage("No vacation found with that ID.");
                return;
            }

            Utility.WriteMessage($"\nVacation found: {vacation.StartDate:yyyy-MM-dd} to {vacation.EndDate:yyyy-MM-dd}");
            Utility.WriteMessage("Enter new vacation dates:");

            var newPeriod = GetVacationPeriodFromUser();

            try
            {
                
                if (vacationService.IsOverlapping(user.Username, newPeriod.StartDate, newPeriod.EndDate))
                {
                    Utility.WriteErrorMessage("New vacation period overlaps with an existing one.");
                    return;
                }
                
                if (vacationService.IsOverlapping(user.Username, newPeriod.StartDate, newPeriod.EndDate))
                {
                    Utility.WriteErrorMessage("New vacation period overlaps with an existing one.");
                    return;
                }
                if (vacationService.IsOverlapping(user.Username, newPeriod.StartDate, newPeriod.EndDate, vacationId))
                {
                    Utility.WriteErrorMessage("New vacation period overlaps with an existing one.");
                    return;
                }

                vacationService.UpdateVacation(vacationId, user.Username, newPeriod);

                operationLog.Add($"{user.Username} updated vacation ID {vacationId} to: {newPeriod.StartDate:yyyy-MM-dd} to {newPeriod.EndDate:yyyy-MM-dd}");
                Utility.WriteMessageWithColor("Vacation updated successfully.\n", ConsoleColor.Green);
                ShowVacationSummary(user, vacationService);
            }
            catch (Exception ex)
            {
                Utility.WriteErrorMessage($"Error: {ex.Message}");
            }
        }

        #endregion

        #region Method to Register Employee Vacation
      
        private static void RegisterEmployeeVacation(VacationService vacationService, List<string> operationLog)
        {
            Utility.WriteTitle("Register Employee's Vacation");

            Utility.WriteMessage("Enter employee username:");
            string employee = Console.ReadLine()?.Trim();

            if (vacationService.UserExists(employee))
            {
                var period = GetVacationPeriodFromUser();

                try
                {
                    if (vacationService.IsOverlapping(employee, period.StartDate, period.EndDate))
                    {
                        Utility.WriteErrorMessage("Vacation period overlaps with existing vacation.");
                        return;
                    }

                    if (vacationService.GetUsedVacationDays(employee) + vacationService.CountWeekdays(period.StartDate, period.EndDate) > 22)
                    {
                        Utility.WriteErrorMessage("Exceeded the maximum of 22 vacation days.");
                        return;
                    }

                    vacationService.RegisterVacation(employee, period);

                    operationLog.Add($"{employee} vacation registered by Admin: {period.StartDate:yyyy-MM-dd} to {period.EndDate:yyyy-MM-dd}");
                    Utility.WriteMessageWithColor("Vacation registered successfully.\n", ConsoleColor.Green);
                }
                catch (Exception ex)
                {
                    Utility.WriteErrorMessage($"Error: {ex.Message}");
                }
            }
            else
            {
                Utility.WriteErrorMessage("Employee does not exist.");
            }
        }
        #endregion

        #region Method to Update Employee Vacation
        private static void UpdateEmployeeVacation(VacationService vacationService, List<string> operationLog)
        {
            Utility.WriteTitle("Update Employee's Vacation");

            Utility.WriteMessage("Enter employee username:");
            string employee = Console.ReadLine()?.Trim();

            if (!vacationService.UserExists(employee))
            {
                Utility.WriteErrorMessage("Employee does not exist.");
                return;
            }

            var vacations = vacationService.GetVacations(employee);
            if (vacations == null || vacations.Count == 0)
            {
                Utility.WriteErrorMessage("No vacations found.");
                return;
            }

            foreach (var v in vacations)
            {
                Utility.WriteMessage($"ID: {v.Id} - {v.StartDate:yyyy-MM-dd} to {v.EndDate:yyyy-MM-dd}");
            }

            Utility.WriteMessage("Enter the vacation ID to update:");
            string input = Console.ReadLine();

            if (!int.TryParse(input, out int vacationId))
            {
                Utility.WriteErrorMessage("Invalid ID.");
                return;
            }

            var existing = vacationService.GetVacationById(vacationId, employee);
            if (existing == null)
            {
                Utility.WriteErrorMessage("Vacation not found.");
                return;
            }

            Utility.WriteMessage($"Vacation found: {existing.StartDate:yyyy-MM-dd} to {existing.EndDate:yyyy-MM-dd}");
            var newPeriod = GetVacationPeriodFromUser();

            try
            {
                if (vacationService.IsOverlapping(employee, newPeriod.StartDate, newPeriod.EndDate))
                {
                    Utility.WriteErrorMessage("New vacation period overlaps with an existing one.");
                    return;
                }
                if (vacationService.IsOverlapping(employee, newPeriod.StartDate, newPeriod.EndDate, vacationId))
                {
                    Utility.WriteErrorMessage("New vacation period overlaps with an existing one.");
                    return;
                }

                vacationService.UpdateVacation(vacationId, employee, newPeriod);

                operationLog.Add($"Admin updated vacation ID {vacationId} for {employee} to {newPeriod.StartDate:yyyy-MM-dd} - {newPeriod.EndDate:yyyy-MM-dd}");
                Utility.WriteMessageWithColor("Vacation updated successfully.\n", ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                Utility.WriteErrorMessage($"Error: {ex.Message}");
            }
        }
        #endregion

        #region Method to ask if user wants to add more vacations
        private static bool AskToAddMoreVacations()
        {
            Utility.WriteMessage("\nDo you want to register more vacations? (y/n): ");
            string response = Console.ReadLine()?.Trim().ToLower();
            return response == "y" || response == "yes";
        }

        private static VacationPeriod GetVacationPeriodFromUser()
        {
            while (true)
            {
                Utility.WriteMessage("Start Date (yyyy-mm-dd): ");
                string startInput = Console.ReadLine();
                Utility.WriteMessage("End Date (yyyy-mm-dd): ");
                string endInput = Console.ReadLine();

                if (DateTime.TryParse(startInput, out DateTime start) &&
                    DateTime.TryParse(endInput, out DateTime end) &&
                    end >= start)
                {
                    return new VacationPeriod(start, end);
                }

                Utility.WriteErrorMessage("\nInvalid dates. Please try again.\n");
            }
        }
        #endregion

        #region Method to show vacation summary
        private static void ShowVacationSummary(User user, VacationService vacationService)
        {
            int used = vacationService.GetUsedVacationDays(user.Username);
            int remaining = vacationService.GetRemainingVacationDays(user.Username);
            Utility.WriteMessage($"Updated Vacation Summary:\nUsed: {used} day(s)\nRemaining: {remaining} day(s)");
        }
        #endregion
    }
}