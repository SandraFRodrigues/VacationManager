using System.Collections.Generic;
using VacationCore.Entities;
using System.Linq;
using D00_Utility;
using System;

namespace VacationCore.Services
{
    public class VacationService
    {
        private const int MaxVacationDays = 22;

        private readonly Dictionary<string, List<VacationPeriod>> _userVacations = new Dictionary<string, List<VacationPeriod>>();
        private readonly LoginService _loginService;
        private int _nextId = 1;

        public VacationService(LoginService loginService)
        {
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
        }

        public List<VacationPeriod> GetVacations(string username)
        {
            string key = username.ToLowerInvariant();

            if (!_userVacations.ContainsKey(key))
            {
                _userVacations[key] = new List<VacationPeriod>();
            }

            return _userVacations[key];
        }

        public int CountWeekdays(DateTime start, DateTime end)
        {
            int count = 0;
            for (var date = start; date <= end; date = date.AddDays(1))
            {
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                {
                    count++;
                }
            }
            return count;
        }

        public int GetUsedVacationDays(string username)
        {
            return GetVacations(username)
                .Sum(v => CountWeekdays(v.StartDate, v.EndDate));
        }

        public int GetRemainingVacationDays(string username)
        {
            return MaxVacationDays - GetUsedVacationDays(username);
        }

        public bool IsOverlapping(string username, DateTime newStart, DateTime newEnd, int? ignoreVacationId = null)
        {
            return GetVacations(username).Any(v =>
                (!ignoreVacationId.HasValue || v.Id != ignoreVacationId.Value) &&
                newStart <= v.EndDate &&
                newEnd >= v.StartDate);
        }

        public void RegisterVacation(string username, VacationPeriod period)
        {
            ValidateVacation(username, period.StartDate, period.EndDate);

            var vacations = GetVacations(username.ToLowerInvariant());
            period.Id = _nextId++;
            vacations.Add(period);
        }

        public VacationPeriod GetVacationById(int id, string username)
        {
            return GetVacations(username)
                .FirstOrDefault(v => v.Id == id);
        }

        public void UpdateVacation(int id, string username, VacationPeriod newPeriod)
        {
            var vacations = GetVacations(username);
            var existing = vacations.FirstOrDefault(v => v.Id == id);

            if (existing == null)
                throw new KeyNotFoundException("\nVacation not found.");

            vacations.Remove(existing);

            try
            {
                ValidateVacation(username, newPeriod.StartDate, newPeriod.EndDate, id);
                existing.StartDate = newPeriod.StartDate;
                existing.EndDate = newPeriod.EndDate;
            }
            finally
            {
                vacations.Add(existing);
            }
        }

        private void ValidateVacation(string username, DateTime start, DateTime end, int? ignoreVacationId = null)
        {
            if (!UserExists(username))
                throw new ArgumentException("\nUser does not exist.");

            if (IsOverlapping(username, start, end, ignoreVacationId))
                throw new InvalidOperationException("\nVacation period overlaps with an existing one.");

            int usedDays = GetUsedVacationDays(username);
            if (ignoreVacationId.HasValue)
            {
                var old = GetVacationById(ignoreVacationId.Value, username);
                if (old != null)
                {
                    usedDays -= CountWeekdays(old.StartDate, old.EndDate);
                }
            }

            int newDays = CountWeekdays(start, end);
            if (usedDays + newDays > MaxVacationDays)
                throw new InvalidOperationException("\nVacation period exceeds maximum allowed days.");
        }

        public bool UserExists(string username)
        {
            return _loginService.GetAllUsers()
                .Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }
    }
}
