﻿namespace Timesheet.Domain.Models
{
    public class EmployeeBenefits
    {
        public decimal AvailableVacation { get; private set; }
        public decimal UsedVacation { get; private set; }
        public decimal ScheduledVacation { get; private set; }
        public decimal AvailablePersonal { get; private set; }
        public decimal UsedPersonnal { get; private set; }
        public decimal ScheduledPersonal { get; private set; }
        public int RolloverHours { get; private set; }
    }
}