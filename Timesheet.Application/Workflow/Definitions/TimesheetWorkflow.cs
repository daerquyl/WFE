﻿using Timesheet.Domain.Models.Employees;
using Timesheet.Domain.Models.Timesheets;

namespace Timesheet.Application.Workflow
{
    internal enum TimesheetTransitions
    {
        SUBMIT, APPROVE, REJECT,FINALIZE
    }

    internal class TimesheetWorkflow : WorkflowDefinition
    {

        public TimesheetWorkflow()
            : base(new List<Transition>()
            {
                new Transition(TimesheetTransitions.SUBMIT, TimesheetStatus.IN_PROGRESS),
                new Transition(TimesheetTransitions.APPROVE, TimeoffStatus.IN_PROGRESS),
                new Transition(TimesheetTransitions.REJECT, TimeoffStatus.IN_PROGRESS),
                new Transition(TimesheetTransitions.FINALIZE, TimeoffStatus.IN_PROGRESS),
            })
        {
        }
    }
}