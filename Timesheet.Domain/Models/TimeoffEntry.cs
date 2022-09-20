﻿namespace Timesheet.Domain.Models
{
    public class TimeoffEntry : Entity
    {
        public TimeoffEntry(string id, DateTime requestDate, TimeoffType type, double hours) : base(id)
        {
            RequestDate = requestDate;
            Type = type;
            Hours = hours;
        }

        public static TimeoffEntry Create(DateTime requestDate, TimeoffType type, double hours)
        {
            var entry = new TimeoffEntry(Guid.NewGuid().ToString(), requestDate, type, hours);
            entry.Status = TimeoffEntryStatus.NOT_PROCESSED;

            return entry;
        }

        public DateTime RequestDate { get; private set; }
        public TimeoffType Type { get; private set; }
        public double Hours { get; private set; }
        public TimeoffEntryStatus Status { get; private set; }

        internal void Validate()
        {
            this.Status = TimeoffEntryStatus.PROCESSED;
        }

        internal void Update(TimeoffType type, double hours)
        {
            this.Type = this.Type;
            this.Hours = hours != 0 ? hours : this.Hours;
        }
    }
}