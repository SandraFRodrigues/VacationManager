using System;
using D00_Utility;
using VacationCore.Entities;

namespace VacationCore.Entities
{
    public class VacationPeriod
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public VacationPeriod() { }

        public VacationPeriod(DateTime start, DateTime end)
        {
            StartDate = start;
            EndDate = end;
        }
    }
}
