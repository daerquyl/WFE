﻿namespace Timesheet.Application.Employees.Commands
{
    public class DeleteTimeoffEntry : ICommand
    {
       public string EmployeeId { get; set; }
       public string TimeoffId { get; set; }
       public string TimeoffEntryId { get; set; }
    }
}