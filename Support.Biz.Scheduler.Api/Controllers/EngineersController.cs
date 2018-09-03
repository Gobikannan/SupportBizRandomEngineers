using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Support.Biz.Scheduler.Repository;
using Support.Biz.Scheduler.Core;

namespace Support.Biz.Scheduler.Api.Controllers
{
    [Route("api/engineers")]
    public class EngineersController : Controller
    {
        private readonly IEngineersRepository engineersRepository;

        public EngineersController(IEngineersRepository engineersRepository)
        {
            this.engineersRepository = engineersRepository;
        }

        [HttpGet]
        public IList<EngineerModel> Get()
        {
            return this.engineersRepository.GetAll();
        }
    }
}
