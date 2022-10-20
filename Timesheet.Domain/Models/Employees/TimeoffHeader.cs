﻿namespace Timesheet.Domain.Models.Employees
{
    public class TimeoffHeader : Entity
    {
        public TimeoffHeader(string id, DateTime requestStartDate,
            DateTime requestEndDate,
            string employeeComment,
            string approverComment
            ) : base(id)
        {
            this.RequestStartDate = requestStartDate;
            this.RequestEndDate = requestEndDate;
            this.EmployeeComment = employeeComment;
            this.ApproverComment = approverComment;
        }

        public DateTime RequestStartDate { get; private set; }
        public DateTime RequestEndDate { get; private set; }
        public string EmployeeComment { get; private set; }
        public string ApproverComment { get; private set; }
        public TimeoffStatus Status { get; private set; }
        public virtual ICollection<TimeoffEntry> TimeoffEntries { get; private set; } = new List<TimeoffEntry>();

        internal static TimeoffHeader Create(DateTime requestStartDate, DateTime requestEndDate, string employeeComment, string supervisorComment)
        {
            var timeoff = new TimeoffHeader(Guid.NewGuid().ToString(), 
                requestStartDate.Date,
                requestEndDate.Date,
                employeeComment,
                supervisorComment);

            timeoff.Status = TimeoffStatus.IN_PROGRESS;

            return timeoff;
        }

        internal void Delete()
        {
            var removedEntries = TimeoffEntries;
            TimeoffEntries = new List<TimeoffEntry>();
        }

        internal void AddEntry(DateTime requestDate, TimeoffType type, double hours, TimeoffHeader timeoff)
        {
            TimeoffEntries.Add(TimeoffEntry.Create(requestDate, type, hours));
        }

        internal void DeleteEntry(TimeoffEntry timeoffEntry)
        {
            TimeoffEntries.Remove(timeoffEntry);
        }

        internal void Approve(string comment)
        {
            Transition(TimeoffStatus.APPROVED, () => this.ApproverComment = comment ?? this.ApproverComment);
            ValidateAllEntries();
        }

        internal void Submit(string comment)
        {
            Transition(TimeoffStatus.SUBMITTED, () => this.EmployeeComment = comment ?? this.EmployeeComment);
        }

        internal void Reject(string comment)
        {
            Transition(TimeoffStatus.REJECTED, () => this.ApproverComment = comment ?? this.ApproverComment);
            ValidateAllEntries();
        }

        internal void Update()
        {
            var timeoffStartDate = TimeoffEntries.Min(e => e.RequestDate);
            var timeoffEndDate = TimeoffEntries.Max(e => e.RequestDate);
            RequestStartDate = timeoffStartDate;
            RequestEndDate = timeoffEndDate;
        }

        private void Transition(TimeoffStatus status, Action updateData)
        {
            this.Status = status;
            updateData();
        }

        private void ValidateAllEntries()
        {
            foreach (var entry in TimeoffEntries)
            {
                entry.Validate();
            }
        }

        public TimeoffEntry? GetTimeoffEntry(string timeoffEntryId) => TimeoffEntries.SingleOrDefault(e => e.Id == timeoffEntryId);

    }
}