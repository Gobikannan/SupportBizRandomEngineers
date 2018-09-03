using System;
using System.Collections.Generic;

namespace Support.Biz.Scheduler.Core
{
    public class Schedule
    {
        public DateTime Date { get; set; }
        public List<Shift> Shifts { get; set; }
    }

    public class Shift
    {
        public DateTime Date { get; set; }
        public ShiftPeriod ShiftPeriod { get; set; }
        public string ShiftPeriodDescription => ShiftPeriod.ToDescription();
        public EngineerModel Engineer { get; set; }
        public string EngineerName => Engineer.FirstName;
    }
}
