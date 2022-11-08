using Interval.MonitorResource;
using Microsoft.AspNetCore.Mvc;

namespace Interval.Api.Controllers
{
    [Route("api")]
    public class MonitoAPIController : ControllerBase
    {

        [HttpGet("processes")]
        public async Task<IActionResult> GetProcessName()
        {
            var result = MonitorResources.GetProcesses();
            return await Task.FromResult(Ok(result));
        }

        [HttpPost("process")]
        public async Task<IActionResult> GetProcess([FromBody]ProcessFilter filter)
        {
            var monitor = new MonitorController(filter.Name);
            var result = monitor.GetProcessConsume();
            return await Task.FromResult(Ok(result));
        }    

    }
}
