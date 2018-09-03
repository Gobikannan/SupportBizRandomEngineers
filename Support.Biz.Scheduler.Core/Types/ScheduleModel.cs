using System;

namespace Support.Biz.Scheduler.Core
{
    public class ScheduleModel
    {
        public int Id { get; set; }
        public string Notes { get; set; }
        public DateTime StartDateTimePeriod { get; set; }
        public DateTime EndDateTimePeriod { get; set; }
    }
}
