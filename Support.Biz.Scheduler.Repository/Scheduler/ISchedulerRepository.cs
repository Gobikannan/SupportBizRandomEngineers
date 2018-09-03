using Support.Biz.Scheduler.Core;
using System;
using System.Collections.Generic;

namespace Support.Biz.Scheduler.Repository
{
    public interface ISchedulerRepository
    {
        List<ScheduleModel> GetAllSchedules();
        ScheduleModel GetSchedule(DateTime startDate, DateTime endDate);
        int CreateSchedule(ScheduleModel schedule);
        void DeleteSchedule(int scheduleId);
        void DeleteAllSchedules();
    }
}
