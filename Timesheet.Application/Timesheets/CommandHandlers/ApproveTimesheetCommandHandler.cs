﻿using Timesheet.Application.Employees.Services;
using Timesheet.Application.Timesheets.Commands;
using Timesheet.Application.TImesheets.CommandHandlers;
using Timesheet.Application.Workflow;
using Timesheet.Domain;
using Timesheet.Domain.Models.Employees;
using Timesheet.Domain.Models.Timesheets;
using Timesheet.Domain.Repositories;

namespace Timesheet.Application.Timesheets.CommandHandlers
{
    internal class ApproveTimesheetCommandHandler : BaseTimesheetCommandHandler<TimesheetHeader, ApproveTimesheet>
    {
        private readonly IWorkflowService _workflowService;

        public ApproveTimesheetCommandHandler(
            IAuditHandler auditHandler,
            IEmployeeReadRepository employeeReadRepository,
            ITimesheetReadRepository readRepository, 
            IWorkflowService workflowService,
            IDispatcher dispatcher,
            IUnitOfWork unitOfWork,
            IEmployeeHabilitation employeeHabilitations
            )
            : base(auditHandler, employeeReadRepository, readRepository, dispatcher, unitOfWork, employeeHabilitations)
        {
            this._workflowService = workflowService;
        }

        public async override Task<IEnumerable<IDomainEvent>> HandleCoreAsync(ApproveTimesheet command, CancellationToken token)
        {
            var employee = await RequireEmployee(command.EmployeeId);

            var timesheet = await RequireTimesheet(command.TimesheetId, employee?.Id);
            this.RelatedAuditableEntity = timesheet;

            //Validate workflow authorization only if timesheet is not finalized yet otherwise this is a regulation
            if (timesheet.Status != TimesheetStatus.FINALIZED)
            {
                var timesheetEntryRef = GetTimesheetFirstData(timesheet);

                EmployeeRoleOnData currentEmployeeRoleOnData = await GetCurrentEmployeeRoleOnData(command, employee);
                _workflowService.AuthorizeTransition(timesheet, TimesheetTransitions.APPROVE, timesheet.Status, currentEmployeeRoleOnData);
                if (timesheetEntryRef is not null)
                {
                    _workflowService.AuthorizeTransition(timesheetEntryRef, TimesheetEntryTransitions.APPROVE, timesheetEntryRef.Status, currentEmployeeRoleOnData);
                }
            }

            timesheet.Approve(employee, command.Comment);

            var events = timesheet.GetDomainEvents();
            timesheet.ClearDomainEvents();

            return events;
        }
    }
}
