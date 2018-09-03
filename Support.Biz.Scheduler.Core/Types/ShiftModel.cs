using System;

namespace Support.Biz.Scheduler.Core
{
    public class ShiftModel
    {
        public int Id { get; set; }
        public int EngineerId { get; set; }
        public int ShiftPeriod { get; set; }
        public int ScheduleId { get; set; }
        public DateTime DateTimePeriod { get; set; }
    }
}
