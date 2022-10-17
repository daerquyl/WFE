﻿using Timesheet.Application.Holidays.Commands;
using Timesheet.Domain;
using Timesheet.Domain.Exceptions;
using Timesheet.Domain.Models.Holidays;
using Timesheet.Domain.Repositories;

namespace Timesheet.Application.Holidays.CommandHandlers
{
    internal class AddHolidayCommandHandler : BaseCommandHandler<Holiday, AddHoliday>
    {
        public readonly IWriteRepository<Holiday> _writeRepository;
        public readonly IHolidayReadRepository _readRepository;

        public AddHolidayCommandHandler(
            IAuditHandler auditHandler,
            IWriteRepository<Holiday> writeRepository,
            IHolidayReadRepository readRepository,
            IDispatcher dispatcher,
            IUnitOfWork unitOfWork
            ) : base(auditHandler, dispatcher, unitOfWork)
        {
            _writeRepository = writeRepository;
            _readRepository = readRepository;
        }

        public override async Task<IEnumerable<IDomainEvent>> HandleCoreAsync(AddHoliday addHoliday, CancellationToken token)
        {
            if (_readRepository.GetByDate(addHoliday.Date) is not null)
            {
                throw new HolidayAlreadyExistException(addHoliday.Date);
            }

            var holiday = Holiday.Create(addHoliday.Date.Date, addHoliday.Description, addHoliday.Notes, addHoliday.IsRecurrent);
            await _writeRepository.Add(holiday);

            this.RelatedAuditableEntity = holiday;

            return holiday.GetDomainEvents();
        }
    }
}
