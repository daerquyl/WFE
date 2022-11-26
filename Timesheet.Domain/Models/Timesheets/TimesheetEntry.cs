﻿using Timesheet.Domain.Models.Employees;

namespace Timesheet.Domain.Models.Timesheets
{
    public class TimesheetEntry : Entity
    {
        public TimesheetEntry(string id, string employeeId, DateTime WorkDate, string payrollCode, double hours,
            string description, string serviceOrderNumber, string serviceOrderDescription, string jobNumber, string jobDescription, string profitCenter, bool outOffCountry, bool isDeletable=false) : base(id)
        {
            EmployeeId = employeeId;
            this.WorkDate = WorkDate;
            PayrollCode = payrollCode;
            Hours = hours;
            Description = description;
            ServiceOrderNumber = serviceOrderNumber;
            ServiceOrderDescription = serviceOrderDescription;
            JobNumber = jobNumber;
            JobDescription = jobDescription;
            ProfitCenterNumber = profitCenter;
            OutOffCountry = outOffCountry;
            this.IsDeletable = isDeletable;
        }

        public TimesheetEntry(string id, string employeeId, DateTime workDate, string payrollCode, double hours,
            string description) : base(id)
        {
            EmployeeId = employeeId.ToString();
            WorkDate = workDate;
            PayrollCode = payrollCode;
            Hours = hours;
            Description = description;
        }


        public string EmployeeId { get; private set; }
        public DateTime WorkDate { get; private set; }
        public string PayrollCode { get; private set; }
        public double Hours { get; private set; }
        public double Quantity => Hours;
        public string Description { get; private set; }
        public string? ServiceOrderNumber { get; private set; }
        public string? ServiceOrderDescription { get; private set; }
        public string? JobNumber { get; private set; }
        public string? JobDescription { get; private set; }
        public string? JobTaskNumber { get; private set; }
        public string? JobTaskDescription { get; private set; }
        public string? LaborCode { get; private set; }
        public string? CustomerNumber { get; private set; }
        public string? ProfitCenterNumber { get; private set; }
        public bool? OutOffCountry { get; private set; } //TODO HERE
        public string? WorkArea => (OutOffCountry ?? false) ? "Out of country" : "In state";
        public bool IsDeletable { get; set; }

        public TimesheetEntryStatus Status { get; set; }

        internal void Submit()
        {
            this.Status = TimesheetEntryStatus.SUBMITTED;
            this.UpdateMetadata();
        }

        internal void Approve()
        {
            this.Status = TimesheetEntryStatus.APPROVED;
            this.UpdateMetadata();
        }

        internal void Reject()
        {
            this.Status = TimesheetEntryStatus.REJECTED;
            this.UpdateMetadata();
        }
    }
}
