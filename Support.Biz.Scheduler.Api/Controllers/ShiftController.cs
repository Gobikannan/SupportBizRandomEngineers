using System;
using Microsoft.AspNetCore.Mvc;
using Support.Biz.Scheduler.Repository;
using Support.Biz.Scheduler.Services;

namespace Support.Biz.Scheduler.Api.Controllers
{
    [Route("api/shift")]
    public class ShiftController : ApiControllerBase
    {
        private readonly IShiftService shiftService;
        private readonly ISchedulerRepository schedulerRepository;

        public ShiftController(IShiftService shiftService, ISchedulerRepository schedulerRepository)
        {
            this.shiftService = shiftService;
            this.schedulerRepository = schedulerRepository;
        }

        /// <summary>
        /// gets the list of schedule based on default condition
        /// the schedule will span two weeks and start on the first working day of the upcoming week. 
        /// </summary>
        /// <returns>Schedules with the shifts based on the startDate & endDate</returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var response = this.shiftService.GenerateShiftsForUpComingWeek();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// gets the list of schedule based on the startDate & endDate
        /// </summary>
        /// <returns>Schedules with the shifts based on the startDate & endDate</returns>
        [HttpGet("{startDate}/{endDate}")]
        public IActionResult GetByCustomDate(DateTime startDate, DateTime endDate)
        {
            try
            {
                var response = this.shiftService.GenerateShifts(startDate, endDate);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Invokes the call to delete all schedules and shifts from the database
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/all/schedules")]
        public IActionResult DeleteAll()
        {
            try
            {
                this.schedulerRepository.DeleteAllSchedules();
                return NoContent();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Invokes the call to delete schedule and shifts based on default startDate and endDate
        /// </summary>
        /// <returns></returns>
        [HttpDelete()]
        public IActionResult Delete()
        {
            try
            {
                this.shiftService.DeleteScheduleForUpcomingWeek();
                return NoContent();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Invokes the call to delete schedule and shifts based on the startDate and endDate
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{startDate}/{endDate}")]
        public IActionResult Delete(DateTime startDate, DateTime endDate)
        {
            try
            {
                this.shiftService.DeleteSchedule(startDate, endDate);
                return NoContent();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
