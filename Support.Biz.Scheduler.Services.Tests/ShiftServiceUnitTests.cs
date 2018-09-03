using Moq;
using Support.Biz.Scheduler.Repository;
using Support.Biz.Scheduler.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Support.Biz.Scheduler.Services.Tests
{
    public class ShiftServiceUnitTests
    {
        private IShiftService shiftService;
        private Mock<ISchedulerRepository> schedulerRepository;
        private Mock<IShiftRepository> shiftRepository;
        private Mock<IEngineersRepository> engineersRepository;

        private void mockEngineers()
        {
            var engineers = new List<EngineerModel>();
            engineers.Add(new EngineerModel { Id = 1, FirstName = "Engineer11", LastName = "11", IsActive = true });
            engineers.Add(new EngineerModel { Id = 2, FirstName = "Engineer12", LastName = "12", IsActive = true });
            engineers.Add(new EngineerModel { Id = 3, FirstName = "Engineer13", LastName = "13", IsActive = true });
            engineers.Add(new EngineerModel { Id = 4, FirstName = "Engineer14", LastName = "14", IsActive = true });
            engineers.Add(new EngineerModel { Id = 5, FirstName = "Engineer15", LastName = "15", IsActive = true });
            engineers.Add(new EngineerModel { Id = 6, FirstName = "Engineer16", LastName = "16", IsActive = true });
            engineers.Add(new EngineerModel { Id = 7, FirstName = "Engineer17", LastName = "17", IsActive = true });
            engineers.Add(new EngineerModel { Id = 8, FirstName = "Engineer18", LastName = "18", IsActive = true });
            engineers.Add(new EngineerModel { Id = 9, FirstName = "Engineer19", LastName = "19", IsActive = true });
            engineers.Add(new EngineerModel { Id = 10, FirstName = "Engineer20", LastName = "20", IsActive = true });
            engineersRepository.Setup(x => x.GetAll()).Returns(engineers);
        }

        public ShiftServiceUnitTests()
        {
            engineersRepository = new Mock<IEngineersRepository>();
            mockEngineers();
            shiftRepository = new Mock<IShiftRepository>();
            schedulerRepository = new Mock<ISchedulerRepository>();
            shiftService = new ShiftService(engineersRepository.Object, schedulerRepository.Object, shiftRepository.Object);
        }

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

        [Fact]
        public void CheckSchedulesCountForDefault()
        {
            var startDate = DateTime.Now;
            do
            {
                startDate = startDate.AddDays(1);
            } while (startDate.DayOfWeek != DayOfWeek.Monday);

            var endDate = startDate.AddDays(11); //10 days but 2 days for weekends 
            var results = this.shiftService.GenerateShifts(startDate, endDate);
            var totalDays = GetWorkingDays(startDate, endDate);
            Assert.Equal(totalDays, results.Count);
        }

        [Theory]
        [InlineData("13-08-2018", "22-08-2018", 8)]
        [InlineData("13-08-2018", "24-08-2018", 10)]
        [InlineData("13-08-2018", "17-08-2018", 5)]
        [InlineData("13-08-2018", "20-08-2018", 6)]
        [InlineData("13-08-2018", "15-08-2018", 3)]
        [InlineData("11-08-2018", "12-08-2018", 0)]
        [InlineData("11-08-2018", "13-08-2018", 1)]
        [InlineData("13-08-2018", "27-08-2018", 11)]
        [InlineData("10-08-2018", "27-08-2018", 12)]
        public void CheckSchedulesCountForMultipleDates(string startDate, string endDate, int expecedTotlDays)
        {
            var results = this.shiftService.GenerateShifts(Convert.ToDateTime(startDate), Convert.ToDateTime(endDate));
            Assert.Equal(expecedTotlDays, results.Count);
        }

        [Fact]
        public void CheckExistingSchedulesCreated()
        {
            var startDate = new DateTime(2018, 08, 13);
            var endDate = new DateTime(2018, 08, 14);

            var schedule = new ScheduleModel { Id = 1, StartDateTimePeriod = startDate.Date, EndDateTimePeriod = endDate.Date };
            schedulerRepository.Setup(x => x.GetSchedule(startDate.Date, endDate.Date)).Returns(schedule);
            var shifts = new List<ShiftModel>
            {
                new ShiftModel { Id = 1, DateTimePeriod = startDate, EngineerId = 1, ScheduleId = schedule.Id, ShiftPeriod = (int)ShiftPeriod.Morning },
                new ShiftModel { Id = 1, DateTimePeriod = startDate, EngineerId = 2, ScheduleId = schedule.Id, ShiftPeriod = (int)ShiftPeriod.Afternoon },
                new ShiftModel { Id = 1, DateTimePeriod = endDate, EngineerId = 3, ScheduleId = schedule.Id, ShiftPeriod = (int)ShiftPeriod.Morning },
                new ShiftModel { Id = 1, DateTimePeriod = endDate, EngineerId = 4, ScheduleId = schedule.Id, ShiftPeriod = (int)ShiftPeriod.Afternoon }
            };

            shiftRepository.Setup(x => x.GetAllShifts(schedule.Id)).Returns(shifts);

            var results = this.shiftService.GenerateShifts(startDate, endDate);
            Assert.Equal(2, results.Count);
        }

        [Fact]
        public void VerifyDeleteExistingScheduleInvoked()
        {
            var startDate = new DateTime(2018, 08, 13);
            var endDate = new DateTime(2018, 08, 14);

            var schedule = new ScheduleModel { Id = 1, StartDateTimePeriod = startDate.Date, EndDateTimePeriod = endDate.Date };
            schedulerRepository.Setup(x => x.GetSchedule(startDate.Date, endDate.Date)).Returns(schedule);
            this.shiftService.DeleteSchedule(startDate, endDate);
            shiftRepository.Verify(x => x.DeleteShift(schedule.Id));
            schedulerRepository.Verify(x => x.DeleteSchedule(schedule.Id));
        }

        [Fact]
        public void VerifyDeleteExistingScheduleNotInvokedIfNotValidSchedule()
        {
            var startDate = new DateTime(2018, 08, 13);
            var endDate = new DateTime(2018, 08, 14);

            var schedule = new ScheduleModel { Id = 1, StartDateTimePeriod = startDate.Date, EndDateTimePeriod = endDate.Date };
            schedulerRepository.Setup(x => x.GetSchedule(startDate.Date, endDate.Date)).Returns(schedule);

            this.shiftService.DeleteSchedule(startDate.AddDays(1), endDate.AddDays(10));
            schedulerRepository.Verify(x => x.GetSchedule(startDate.AddDays(1), endDate.AddDays(10)));
            schedulerRepository.VerifyNoOtherCalls();
            shiftRepository.VerifyNoOtherCalls();
        }
    }
}
