
namespace Patheyam.Web.API.Controllers.ServiceMonitoring
{
    using Patheyam.Domain.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Service Monitor Nudge
    /// </summary>
    [Route("api/v1/nudge")]
    [ApiController]
    public class NudgeController : ControllerBase
    {
        private readonly Collection<INudgeEngine> _nudgeEngine;
        private readonly ILogger<NudgeController> _logger;
        public NudgeController(Collection<INudgeEngine> nudgeEngine, ILogger<NudgeController> logger)
        {
            _logger = logger;
            _nudgeEngine = nudgeEngine;
        }

        /// <summary>
        /// Used By Service Monitoring to Determine if Service is running/can be communicated with
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route(""), AllowAnonymous]
        public bool Get()
        {
            var finalReason = string.Empty;
            var result = true;
            try
            {
                foreach (var nudge in _nudgeEngine)
                {
                    result &= nudge.Nudge(out var reason);
                    finalReason += reason;
                }
                if (!result)
                {
                    _logger.LogError($"Nudge request failed: {finalReason}");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                result = false;
            }
            return result;
        }
    }
}
