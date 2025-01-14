﻿using Timesheet.Domain.Exceptions;
using Timesheet.Domain.Models;
using Timesheet.Domain.Models.Holidays;
using Timesheet.Domain.Models.Notifications;
using Timesheet.Domain.Repositories;

namespace Timesheet.Infrastructure.Persistence.Repositories
{
    internal class NotificationReadRepository : ReadRepository<Notification>, INotificationReadRepository
    {
        public NotificationReadRepository(TimesheetDbContext context) : base(context)
        {
        }

        public Holiday? GetByDate(DateTime date)
        {
            var holiday = _context.Holidays.FirstOrDefault(e => e.Date.ToShortDateString == date.ToShortDateString);
            if(holiday == null)
            {
                throw new HolidayAlreadyExistException(date);
            }
            return holiday;
        }

        public Notification? GetByGroupAndAction(NotificationType group, string action)
        {
            return _context.Notifications.FirstOrDefault(e => e.Group == group && e.Action == action);
        }
    }
}
