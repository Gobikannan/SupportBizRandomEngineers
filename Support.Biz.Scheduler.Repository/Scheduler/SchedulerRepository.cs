using Dapper;
using Microsoft.Extensions.Options;
using Support.Biz.Scheduler.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Support.Biz.Scheduler.Repository
{
    public class SchedulerRepository : ISchedulerRepository
    {
        private readonly IOptions<AppSettingsConfig> connectionString;

        public SchedulerRepository(IOptions<AppSettingsConfig> connectionString)
        {
            this.connectionString = connectionString;
        }

        private string GetConnectionString()
        {
            return this.connectionString.Value.ConnectionString;
        }

        public List<ScheduleModel> GetAllSchedules()
        {
            using (IDbConnection db = new SqlConnection(GetConnectionString()))
            {
                return db.Query<ScheduleModel>("select * from Schedule").ToList();
            }
        }

        public ScheduleModel GetSchedule(DateTime startDate, DateTime endDate)
        {
            using (IDbConnection db = new SqlConnection(GetConnectionString()))
            {
                return db.Query<ScheduleModel>("select top 1 * from Schedule where StartDateTimePeriod = @startDate and EndDateTimePeriod = @endDate", new { startDate, endDate  }).FirstOrDefault();
            }
        }

        public int CreateSchedule(ScheduleModel schedule)
        {
            using (IDbConnection db = new SqlConnection(GetConnectionString()))
            {
                const string sql = @"insert into Schedule(StartDateTimePeriod, EndDateTimePeriod, Notes) values(@StartDateTimePeriod, @EndDateTimePeriod, @Notes);SELECT CAST(SCOPE_IDENTITY() AS INT)";
                var id = db.Query<int>(sql, schedule).Single();
                return id;
            }
        }

        public void DeleteSchedule(int scheduleId)
        {
            using (IDbConnection db = new SqlConnection(GetConnectionString()))
            {
                db.Execute(@"delete from Schedule where Id = @scheduleId", new { scheduleId });
            }
        }

        public void DeleteAllSchedules()
        {
            using (IDbConnection db = new SqlConnection(GetConnectionString()))
            {
                db.Execute(@"delete from Shift");
                db.Execute(@"delete from Schedule");
            }
        }
    }
}
