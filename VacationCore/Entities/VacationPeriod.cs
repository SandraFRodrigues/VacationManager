using System;

namespace VacationCore.Entities
{
    public class VacationPeriod
    {
        private static int _nextId = 1; // contador interno de IDs

        public int Id { get; private set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public VacationPeriod(DateTime startDate, DateTime endDate)
        {
            Id = _nextId++; // atribui ID automaticamente
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}