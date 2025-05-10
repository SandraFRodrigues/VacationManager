using System;
using System.Collections.Generic;
using VacationCore.Entities;
using System.Linq;
using D00_Utility;

namespace VacationCore.Services
{
    public class VacationService
    {
        private readonly Dictionary<string, List<VacationPeriod>> _vacations;
        private const int MaxVacationDaysPerYear = 22;

        public VacationService()
        {
            _vacations = new Dictionary<string, List<VacationPeriod>>();
        }

        public void RegisterVacation(string username, VacationPeriod period)
        {
            if (!_vacations.ContainsKey(username))
            {
                _vacations[username] = new List<VacationPeriod>();
            }

            _vacations[username].Add(period);
        }

        public bool UpdateVacation(string username, int vacationId, VacationPeriod newPeriod)
        {
            if (_vacations.TryGetValue(username, out var periods))
            {
                var vacation = periods.FirstOrDefault(v => v.Id == vacationId);
                if (vacation != null)
                {
                    vacation.StartDate = newPeriod.StartDate;
                    vacation.EndDate = newPeriod.EndDate;
                    return true;
                }
            }
            return false;
        }

        public List<VacationPeriod> GetVacations(string username)
        {
            if (_vacations.TryGetValue(username, out var vacations))
            {
                return vacations;
            }
            return new List<VacationPeriod>();
        }

        public int GetUsedVacationDays(string username)
        {
            if (!_vacations.ContainsKey(username))
                return 0;

            return _vacations[username]
                .Where(v => v.StartDate.Year == DateTime.Today.Year) // apenas ano atual
                .Sum(v => CountWeekdays(v.StartDate, v.EndDate));
        }

        public int GetRemainingVacationDays(string username)
        {
            int used = GetUsedVacationDays(username);
            return Math.Max(0, MaxVacationDaysPerYear - used);
        }

        public int CountWeekdays(DateTime start, DateTime end)
        {
            int count = 0;
            for (DateTime date = start; date <= end; date = date.AddDays(1))
            {
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                {
                    count++;
                }
            }
            return count;
        }
    }
}

