using System;
using D00_Utility;

namespace VacationCore.Entities
{
    public class Vacation
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Vacation(int id, DateTime start, DateTime end)
        {
            Id = id;
            StartDate = start;
            EndDate = end;
        }
    }
}