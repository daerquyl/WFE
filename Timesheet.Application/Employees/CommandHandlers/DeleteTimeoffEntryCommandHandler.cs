﻿using Timesheet.Application.Employees.Commands;
using Timesheet.Application.Workflow;
using Timesheet.Domain.Models;
using Timesheet.Domain.Repositories;

namespace Timesheet.Application.Employees.CommandHandlers
{
    internal class DeleteTimeoffEntryCommandHandler : BaseEmployeeCommandHandler<DeleteTimeoffEntry>
    {
        private readonly IReadRepository<Employee> _readRepository;
        private readonly IWorkflowService _workflowService;

        public DeleteTimeoffEntryCommandHandler(
            IReadRepository<Employee> readRepository,
            IWorkflowService workflowService,
            IDispatcher dispatcher,
            IUnitOfWork unitOfWork
            ) : base(readRepository, dispatcher, unitOfWork)
        {
            _readRepository = readRepository;
            _workflowService = workflowService;
        }

        public override async Task<IEnumerable<IDomainEvent>> HandleCore(DeleteTimeoffEntry command, CancellationToken token)
        {
            var employee = await GetEmployee(command.EmployeeId);
            var timeoff = GetTimeoffOrThrowException(employee, command.TimeoffId);
            var timeoffEntry = GetTimeoffEntryOrThrowException(employee, timeoff, command.TimeoffEntryId);

            _workflowService.AuthorizeTransition(timeoff, TimeoffTransitions.DELETE_ENTRY, timeoff.Status);
            _workflowService.AuthorizeTransition(timeoffEntry, TimeoffEntryTransitions.DELETE, timeoffEntry.Status);

            employee.DeleteTimeoffEntry(timeoff, timeoffEntry);

            return employee.GetDomainEvents();
        }
    }
}
