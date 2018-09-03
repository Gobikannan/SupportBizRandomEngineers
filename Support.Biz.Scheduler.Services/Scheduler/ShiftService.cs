using Support.Biz.Scheduler.Core;
using Support.Biz.Scheduler.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Support.Biz.Scheduler.Services
{
    /// <summary>
    /// Schedule service to generate Shifts based on the user input
    /// </summary>
    public class ShiftService : IShiftService
    {
        private readonly IEngineersRepository engineersRepository;
        private readonly ISchedulerRepository schedulerRepository;
        private readonly IShiftRepository shiftRepository;

        public ShiftService(IEngineersRepository engineersRepository, ISchedulerRepository schedulerRepository, IShiftRepository shiftRepository)
        {
            this.engineersRepository = engineersRepository;
            this.schedulerRepository = schedulerRepository;
            this.shiftRepository = shiftRepository;
        }

        /// <summary>
        /// To process shifts from existing schedule created
        /// </summary>
        /// <param name="schedule"></param>
        /// <returns>List of schedules with shifts for each day</returns>
        private List<Schedule> GetExistingSchedules(ScheduleModel schedule)
        {
            if (schedule == null)
            {
                throw new ArgumentNullException(nameof(schedule));
            }
            var totalSchedulers = new List<Schedule>();
            var engineers = this.engineersRepository.GetAll();

            var shifts = this.shiftRepository.GetAllShifts(schedule.Id);
            var uniqueDates = shifts.Select(s => s.DateTimePeriod).Distinct().ToList();

            foreach (var uniqueDate in uniqueDates)
            {
                var scd = new Schedule();
                var tdyShifts = shifts.Where(shft => shft.DateTimePeriod == uniqueDate)
                    .Select(x => new Shift { Date = uniqueDate, ShiftPeriod = (ShiftPeriod)x.ShiftPeriod, Engineer = engineers.FirstOrDefault(y => y.Id == x.EngineerId) }).ToList();
                scd.Date = uniqueDate;
                scd.Shifts = tdyShifts;
                totalSchedulers.Add(scd);
            }

            return totalSchedulers;
        }

        /// <summary>
        /// Calculate max shifts per week for an engineer, considering 5 days a week
        /// </summary>
        /// <param name="totalDays"></param>
        /// <returns>an engineer max allowed per week</returns>
        private int AllowMaxShiftPerEngineer(double totalDays)
        {
            var workingDaysPerWeek = 5;
            return (int)((totalDays / workingDaysPerWeek) + (totalDays % workingDaysPerWeek));
        }

        /// <summary>
        /// Delete the schedule and shifts based for upcoming week
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public void DeleteScheduleForUpcomingWeek()
        {
            var startDate = GetStartDate();
            var endDate = GetEndDate(startDate);

            DeleteSchedule(startDate, endDate);
        }

        /// <summary>
        /// Delete the schedule and shifts based on startDate and endDate
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public void DeleteSchedule(DateTime startDate, DateTime endDate)
        {
            startDate = startDate.Date;
            endDate = endDate.Date;

            //TODO: this one makes many db call to delete. 
            //it could be handle with sql between command. if the range insert as very big, like a year, then it will do something like 600+ call to db to delete row by row      
            var schedule = this.schedulerRepository.GetSchedule(startDate, endDate);
            if (schedule != null)
            {
                //deletes all shifts first
                this.shiftRepository.DeleteShift(schedule.Id);
                //deletes the schedule
                this.schedulerRepository.DeleteSchedule(schedule.Id);
            }
        }

        /// <summary>
        /// Calculate working days by excluding sunday & saturday
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private int GetWorkingDays(DateTime startDate, DateTime endDate)
        {
            int count = 0;
            for (DateTime i = startDate; i <= endDate; i = i.AddDays(1))
            {
                //exclude weekends
                if (i.DayOfWeek == DayOfWeek.Sunday || i.DayOfWeek == DayOfWeek.Saturday)
                {
                    continue;
                }
                count++;
            }
            return count;
        }

        /// <summary>
        /// Gets the first day of upcoming week
        /// the schedule will span two weeks and start on the first working day of the upcoming week. 
        /// </summary>
        /// <returns></returns>
        private DateTime GetStartDate()
        {
            var startDate = DateTime.Now;
            do
            {
                startDate = startDate.AddDays(1);
            } while (startDate.DayOfWeek != DayOfWeek.Monday);

            return startDate;
        }

        /// <summary>
        /// Gets the endDate based on start date
        /// 11 = 10 working days by excluding 2 days for weekends and addition one to include the first/last day
        /// </summary>
        /// <param name="startDate"></param>
        /// <returns></returns>
        private DateTime GetEndDate(DateTime startDate)
        {
            var endDate = startDate.AddDays(11);
            return endDate;
        }

        /// <summary>
        /// Generate shifts based on upcoming week
        /// </summary>
        /// <returns>list of schedules with shifts</returns>
        public List<Schedule> GenerateShiftsForUpComingWeek()
        {
            var startDate = GetStartDate();
            var endDate = GetEndDate(startDate);
            return GenerateShifts(startDate, endDate);
        }

        /// <summary>
        /// Generate shifts based on startDate and endDate
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>list of schedules with shifts</returns>
        public List<Schedule> GenerateShifts(DateTime startDate, DateTime endDate)
        {
            //to have only dates and ignore time
            startDate = startDate.Date;
            endDate = endDate.Date;

            //gets the list of engineers
            var engineers = this.engineersRepository.GetAll();

            //calculate total working days exlcuding weekends
            var totalDays = GetWorkingDays(startDate, endDate);
            var totalShifts = new List<Shift>();
            var totalSchedulers = new List<Schedule>();

            var schedule = this.schedulerRepository.GetSchedule(startDate, endDate);
            if (schedule != null)
            {
                totalSchedulers = GetExistingSchedules(schedule);
                return totalSchedulers;
            }

            //to find out if the engineer has same allocated previous day noon shift
            var previousNoonEngineer = new EngineerModel { Id = 0 };

            //allow one shift for a week and calculate based on the week you move on
            var maxAllowedCount = 1;

            //create schedule for the given dates
            var scheduleId = this.schedulerRepository.CreateSchedule(new ScheduleModel { StartDateTimePeriod = startDate, EndDateTimePeriod = endDate });

            for (DateTime i = startDate; i <= endDate; i = i.AddDays(1))
            {
                //exclude weekends
                if (i.DayOfWeek == DayOfWeek.Sunday || i.DayOfWeek == DayOfWeek.Saturday)
                {
                    continue;
                }

                //remove if engineers have more than 2 shifts already
                if (i != startDate && i.DayOfWeek == DayOfWeek.Monday)
                {
                    maxAllowedCount++;
                }

                var randNum = new Random();
                //choose random EngineerOne
                EngineerModel engineerOne = null;
                //choose random EngineerOne after checking if he has enough slots for a period
                do
                {
                    engineerOne = engineers[randNum.Next(engineers.Count)];
                } while (totalShifts.Count(shft => engineerOne != null && shft.Engineer.Id == engineerOne.Id) >= maxAllowedCount);

                //choose random EngineerTwo
                EngineerModel engineerTwo = null;
                //to avoid overflow in case if the random same engineer fall in last two days
                var overflowCnt = 0;
                //choose random EngineerTwo after checking if he has enough slots for a period and also if EngineerTwo is not previous day noon shifter
                do
                {
                    engineerTwo = engineers[randNum.Next(engineers.Count)];
                    overflowCnt++;
                    if (overflowCnt > 100)
                    {
                        break;
                    }
                } while (engineerTwo != null && (engineerTwo.Id == engineerOne.Id || engineerTwo.Id == previousNoonEngineer.Id || totalShifts.Count(shft => shft.Engineer.Id == engineerTwo.Id) >= maxAllowedCount));

                //store the noon shift engineer to compare next day
                previousNoonEngineer = engineerTwo;

                var morningShift = new Shift { Date = i, ShiftPeriod = ShiftPeriod.Morning, Engineer = engineerOne };
                var afternoonShift = new Shift { Date = i, ShiftPeriod = ShiftPeriod.Afternoon, Engineer = engineerTwo };
                totalShifts.Add(morningShift);
                totalShifts.Add(afternoonShift);

                RemoveEngineerIfExceedsLimit(engineers, totalShifts, totalDays, engineerOne);
                RemoveEngineerIfExceedsLimit(engineers, totalShifts, totalDays, engineerTwo);

                totalSchedulers.Add(new Schedule { Date = i, Shifts = new List<Shift> { morningShift, afternoonShift } });
            }

            //store the shifts for the schedule created above
            var allShifts = totalShifts.Select(shift => new ShiftModel { ScheduleId = scheduleId, EngineerId = shift.Engineer.Id, ShiftPeriod = (int)shift.ShiftPeriod, DateTimePeriod = shift.Date }).ToList();
            this.shiftRepository.CreateShift(allShifts);

            //return the schedulers
            return totalSchedulers;
        }

        /// <summary>
        /// Remove the engineer if it is already has enough slots based on total days
        /// </summary>
        /// <param name="engineers"></param>
        /// <param name="totalShifts"></param>
        /// <param name="totalDays"></param>
        /// <param name="engineer"></param>
        private void RemoveEngineerIfExceedsLimit(List<EngineerModel> engineers, List<Shift> totalShifts, double totalDays, EngineerModel engineer)
        {
            var count = totalShifts.Count(shft => shft.Engineer.Id == engineer.Id);
            if (count == AllowMaxShiftPerEngineer(totalDays))
            {
                engineers.Remove(engineer);
            }
        }
    }
}
