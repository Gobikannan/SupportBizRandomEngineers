using Dapper;
using Microsoft.Extensions.Options;
using Support.Biz.Scheduler.Core;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Support.Biz.Scheduler.Repository
{
    public class ShiftRepository : IShiftRepository
    {
        private readonly IOptions<AppSettingsConfig> connectionString;

        public ShiftRepository(IOptions<AppSettingsConfig> connectionString)
        {
            this.connectionString = connectionString;
        }

        private string GetConnectionString()
        {
            return this.connectionString.Value.ConnectionString;
        }

        public List<ShiftModel> GetAllShifts()
        {
            using (IDbConnection db = new SqlConnection(GetConnectionString()))
            {
                return db.Query<ShiftModel>("select * from Shift").ToList();
            }
        }

        public List<ShiftModel> GetAllShifts(int scheduleId)
        {
            using (IDbConnection db = new SqlConnection(GetConnectionString()))
            {
                return db.Query<ShiftModel>("select * from Shift where ScheduleId = @scheduleId", new { scheduleId }).ToList();
            }
        }

        public int CreateShift(List<ShiftModel> shifts)
        {
            using (IDbConnection db = new SqlConnection(GetConnectionString()))
            {
                var id = db.Execute(@"insert into Shift(DateTimePeriod, ScheduleId, ShiftPeriod, EngineerId) values(@DateTimePeriod, @ScheduleId, @ShiftPeriod, @EngineerId)", shifts);
                return id;
            }
        }

        public void DeleteShift(int scheduleId)
        {
            using (IDbConnection db = new SqlConnection(GetConnectionString()))
            {
                db.Execute(@"delete from Shift where ScheduleId = @ScheduleId", new { ScheduleId = scheduleId });
            }
        }
    }
}
