using System.ComponentModel;

namespace Support.Biz.Scheduler.Core
{
    public enum ShiftPeriod
    {
        [Description("Morning Shift")]
        Morning = 1,
        [Description("Afternoon Shift")]
        Afternoon = 2
    }
}
