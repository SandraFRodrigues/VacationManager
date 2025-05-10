using D00_Utility;
using System;
using VacationCore.Entities;

namespace VacationApp.Views
{
    public static class VacationInputView
    {
        public static VacationPeriod GetVacationPeriod()
        {
            while (true)
            {
                Console.Write("Start Date (yyyy-mm-dd): ");
                string startInput = Console.ReadLine();
                Console.Write("End Date (yyyy-mm-dd): ");
                string endInput = Console.ReadLine();

                if (DateTime.TryParse(startInput, out DateTime start) &&
                    DateTime.TryParse(endInput, out DateTime end))
                {
                    if (start < DateTime.Today || end < DateTime.Today || end.Year > DateTime.Today.Year)
                    {
                        Console.WriteLine("Invalid dates. Vacation must be from today and within the current year.");
                        continue;
                    }
                    return new VacationPeriod(start, end);
                }
                else
                {
                    Console.WriteLine("Invalid date format. Try again.");
                }
            }
        }
    }
}
