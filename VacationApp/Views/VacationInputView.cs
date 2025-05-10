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

                if (DateUtility.TryParseVacationDates(startInput, endInput, out DateTime start, out DateTime end))
                {
                    return new VacationPeriod(start, end);
                }
            }
        }
    }
}