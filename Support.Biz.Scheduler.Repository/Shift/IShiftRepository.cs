using Support.Biz.Scheduler.Core;
using System.Collections.Generic;

namespace Support.Biz.Scheduler.Repository
{
    public interface IShiftRepository
    {
        List<ShiftModel> GetAllShifts();
        int CreateShift(List<ShiftModel> shifts);
        List<ShiftModel> GetAllShifts(int scheduleId);
        void DeleteShift(int scheduleId);
    }
}
