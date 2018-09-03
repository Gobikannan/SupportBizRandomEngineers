using Dapper;
using Microsoft.Extensions.Options;
using Support.Biz.Scheduler.Core;
using Support.Biz.Scheduler.Core;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Support.Biz.Scheduler.Repository
{
    public class EngineersRepository : IEngineersRepository
    {
        IOptions<AppSettingsConfig> connectionString;

        public EngineersRepository(IOptions<AppSettingsConfig> connectionString)
        {
            this.connectionString = connectionString;
        }

        private string GetConnectionString()
        {
            return this.connectionString.Value.ConnectionString;
        }

        public List<EngineerModel> GetAll()
        {
            using (IDbConnection db = new SqlConnection(GetConnectionString()))
            {
                return db.Query<EngineerModel>("select * from Engineer").ToList();
            }
        }
    }
}
