using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D00_Utility;

namespace D00_Utility
{
    //método para validar as datas de férias
    public static class DateUtility
    {
        public static bool TryParseVacationDates(string startInput, string endInput, out DateTime startDate, out DateTime endDate)
        {
            startDate = default;
            endDate = default;

            if (!DateTime.TryParse(startInput, out startDate) || !DateTime.TryParse(endInput, out endDate))
            {
                Utility.WriteErrorMessage("Invalid date format. Try again.\n\n");
                return false;
            }

            if (!IsVacationDateValid(startDate, endDate))
            {
                return false;
            }

            return true;
        }

        public static bool IsVacationDateValid(DateTime start, DateTime end)
        {
            if (end < start)
            {
                Utility.WriteErrorMessage("End date cannot be earlier than start date.\n\n");
                return false;
            }

            if (start < DateTime.Today || end < DateTime.Today)
            {
                Utility.WriteErrorMessage("Dates must not be in the past.\n\n");
                return false;
            }

            if (start.Year != DateTime.Today.Year || end.Year != DateTime.Today.Year)
            {
                Utility.WriteErrorMessage("Vacation must be within the current year.\n\n");
                return false;
            }

            return true;
        }
    }
}
