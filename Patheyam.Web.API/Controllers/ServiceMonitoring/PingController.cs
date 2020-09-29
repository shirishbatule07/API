
namespace Patheyam.Web.Api.Controllers.ServiceMonitoring
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Service Monitor Ping
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    {
        /// <summary>
        /// Used By Service Monitoring to Determine if Service is running/can be communicated with
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route(""), AllowAnonymous]
        public bool Get()
        {
            return true;
        }
    }
}
