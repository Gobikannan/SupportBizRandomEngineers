using Support.Biz.Scheduler.Core;
using System;
using System.Collections.Generic;

namespace Support.Biz.Scheduler.Services
{
    public interface IShiftService
    {
        List<Schedule> GenerateShiftsForUpComingWeek();
        List<Schedule> GenerateShifts(DateTime startDate, DateTime endDate);
        void DeleteSchedule(DateTime startDate, DateTime endDate);
        void DeleteScheduleForUpcomingWeek();
    }
}
