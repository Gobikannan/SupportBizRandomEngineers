using Support.Biz.Scheduler.Core;
using System.Collections.Generic;

namespace Support.Biz.Scheduler.Repository
{
    public interface IEngineersRepository
    {
        List<EngineerModel> GetAll();
    }
}
