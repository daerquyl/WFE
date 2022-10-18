﻿using Timesheet.Application.Settings.Commands;
using Timesheet.Domain;
using Timesheet.Domain.Exceptions;
using Timesheet.Domain.Models.Settings;
using Timesheet.Domain.Repositories;

namespace Timesheet.Application.Settings.CommandHandlers
{
    internal class UpdateSettingCommandHandler : BaseCommandHandler<Setting, UpdateSetting>
    {
        public readonly IWriteRepository<Setting> _writeRepository;
        public readonly IReadRepository<Setting> _readRepository;

        public UpdateSettingCommandHandler(
            IAuditHandler auditHandler,
            IWriteRepository<Setting> writeRepository,
            IReadRepository<Setting> readRepository,
            IDispatcher dispatcher,
            IUnitOfWork unitOfWork
            ) : base(auditHandler, dispatcher, unitOfWork)
        {
            _writeRepository = writeRepository;
            _readRepository = readRepository;
        }

        public override async Task<IEnumerable<IDomainEvent>> HandleCoreAsync(UpdateSetting updateSetting, CancellationToken token)
        {
            if (updateSetting.Id == null)
            {
                throw new EntityNotFoundException<Setting>(updateSetting.Id);
            }

            var existingSetting = await _readRepository.Get(updateSetting.Id);
            if (existingSetting is null)
            {
                throw new EntityNotFoundException<Setting>(updateSetting.Id);
            }

            existingSetting.Update(updateSetting.Name, updateSetting.Value);

            this.RelatedAuditableEntity = existingSetting;

            return Enumerable.Empty<IDomainEvent>() ;
        }
    }
}
