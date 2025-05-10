using System;
using VacationCore.Entities;
using VacationCore.Services;
using VacationApp.Views;
using D00_Utility;
using System.Collections.Generic;

namespace VacationApp
{
    public static class MainMenu
    {
        private const int MaxVacationDaysPerYear = 22;
        public static void Show(User user, VacationService vacationService, List<string> operationLog)
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Utility.WriteTitle("Vacation Manager");

                Utility.WriteMessage($"\nWelcome, {user.Username}!\n");
                Utility.ShowGreeting();

                int used = vacationService.GetUsedVacationDays(user.Username);
                int remaining = vacationService.GetRemainingVacationDays(user.Username);

                Utility.WriteMessage($"\nVacation Summary for {user.Username}:\n");
                Utility.WriteMessage($"Used: {used} day(s)\n");
                Utility.WriteMessage($"Remaining: {remaining} day(s)\n");

                Console.WriteLine();
                foreach (string option in user.GetMenuOptions())
                {
                    Utility.WriteMessage(option + "\n");
                }

                Utility.WriteMessage("\nSelect option: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Utility.WriteTitle("Register Vacation");
                        bool addMore;
                        do
                        {
                            var period = GetVacationPeriodFromUser();

                            int weekdaysUsed = vacationService.CountWeekdays(period.StartDate, period.EndDate);

                            if (weekdaysUsed > remaining)
                            {
                                Utility.WriteErrorMessage($"You are trying to use {weekdaysUsed} weekdays, but only {remaining} are available.");
                                break;
                            }

                            vacationService.RegisterVacation(user.Username, period);
                            operationLog.Add($"{user.Username} registered vacation: {period.StartDate:yyyy-MM-dd} to {period.EndDate:yyyy-MM-dd}");
                            Utility.WriteMessage("Vacation registered successfully.\n");
                            Utility.WriteMessage($"You used {weekdaysUsed} weekday(s) in this vacation period.\n");

                            ShowVacationSummary(user, vacationService);

                            addMore = AskToAddMoreVacations();
                        } while (addMore);
                        break;

                    case "2":
                        Utility.WriteTitle("List Own Vacations");
                        var vacations = vacationService.GetVacations(user.Username);
                        foreach (var v in vacations)
                        {
                            Utility.WriteMessage($"ID: {v.Id} - {v.StartDate:yyyy-MM-dd} to {v.EndDate:yyyy-MM-dd}\n");
                        }
                        break;

                    case "3":
                        Utility.WriteTitle("Update Vacation");
                        var userVacations = vacationService.GetVacations(user.Username);
                        foreach (var v in userVacations)
                        {
                            Utility.WriteMessage($"ID: {v.Id} - {v.StartDate:yyyy-MM-dd} to {v.EndDate:yyyy-MM-dd}\n");
                        }

                        Utility.WriteMessage("Enter ID to update: ");
                        if (int.TryParse(Console.ReadLine(), out int updateId))
                        {
                            var newPeriod = GetVacationPeriodFromUser();
                            if (vacationService.UpdateVacation(user.Username, updateId, newPeriod))
                            {
                                int updatedDays = vacationService.CountWeekdays(newPeriod.StartDate, newPeriod.EndDate);
                                operationLog.Add($"{user.Username} updated vacation ID {updateId}: {newPeriod.StartDate:yyyy-MM-dd} to {newPeriod.EndDate:yyyy-MM-dd}");
                                Utility.WriteMessage("Vacation updated.\n");
                                Utility.WriteMessage($"New period uses {updatedDays} weekday(s).\n");
                            }
                            else
                            {
                                Utility.WriteErrorMessage("Vacation not found.");
                            }
                        }
                        else
                        {
                            Utility.WriteErrorMessage("Invalid ID.");
                        }
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

                if (!exit)
                {
                    Utility.PauseConsole();
                }
            }
        }

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
                    DateTime.TryParse(endInput, out DateTime end))
                {
                    if (start < DateTime.Today || end < DateTime.Today || end.Year > DateTime.Today.Year)
                    {
                        Utility.WriteErrorMessage("Invalid dates. Vacation must be from today and within the current year.");
                        continue;
                    }

                    return new VacationPeriod(start, end);
                }
                else
                {
                    Utility.WriteErrorMessage("Invalid date format. Try again.");
                }
            }
        }

        private static void ShowVacationSummary(User user, VacationService vacationService)
        {
            int used = vacationService.GetUsedVacationDays(user.Username);
            int remaining = vacationService.GetRemainingVacationDays(user.Username);
            Utility.WriteMessage($"\nUpdated Vacation Summary:\nUsed: {used} day(s), Remaining: {remaining} day(s)\n");
        }
    }
}