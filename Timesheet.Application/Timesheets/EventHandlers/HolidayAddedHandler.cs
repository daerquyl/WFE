﻿using Timesheet.Domain.DomainEvents;

namespace Timesheet.Application.Timesheets.EventHandlers
{
    internal sealed class HolidayAddedHandler : IEventHandler<HolidayAdded>
    {
        public Task Handle(HolidayAdded @event)
        {
            throw new NotImplementedException();
        }
    }
}
