﻿using Timesheet.Application.Employees.Services;
using Timesheet.Application.Notifications.Commands;
using Timesheet.Domain;
using Timesheet.Domain.Exceptions;
using Timesheet.Domain.Models.Notifications;
using Timesheet.Domain.Repositories;

namespace Timesheet.Application.Notifications.CommandHandlers
{
    internal class UpdateNotificationCommandHandler : BaseCommandHandler<Notification, UpdateNotification>
    {
        public readonly IWriteRepository<Notification> _writeRepository;
        public readonly INotificationReadRepository _readRepository;

        public UpdateNotificationCommandHandler(
            IEmployeeReadRepository employeeReadRepository,
            IAuditHandler auditHandler,
            IDispatcher dispatcher,
            IUnitOfWork unitOfWork,
            IWriteRepository<Notification> writeRepository,
            INotificationReadRepository readRepository,
            IEmployeeHabilitation employeeHabilitation
            ) 
            : base(employeeReadRepository, auditHandler, dispatcher, unitOfWork, employeeHabilitation)
        {
            _writeRepository = writeRepository;
            _readRepository = readRepository;
        }

        public override async Task<IEnumerable<IDomainEvent>> HandleCoreAsync(UpdateNotification updateNotification, CancellationToken token)
        {
            if (updateNotification.Id == null)
            {
                throw new EntityNotFoundException<Notification>(updateNotification.Id);
            }

            var existingNotification = await _readRepository.Get(updateNotification.Id);
            if (existingNotification is null)
            {
                throw new EntityNotFoundException<Notification>(updateNotification.Id);
            }

            existingNotification.UpdatePopulation(updateNotification.Population);

            this.RelatedAuditableEntity = existingNotification;

            return Enumerable.Empty<IDomainEvent>();
        }
    }
}
