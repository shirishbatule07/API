
namespace Patheyam.Web.Api.Controllers
{
    using Api.Utils;
    using CSharpFunctionalExtensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    [Authorize]
    public class BaseController : ControllerBase
    {
        protected new IActionResult Ok()
        {
            return base.Ok(Envelope.Ok());
        }

        protected IActionResult Ok<T>(T result)
        {
            return base.Ok(Envelope.Ok(result));
        }

        protected IActionResult Error(string errorMessage)
        {
            return BadRequest(Envelope.Error(errorMessage));
        }

        protected IActionResult FromResult(Result result)
        {
            return result.IsSuccess ? Ok() : Error(result.Error);
        }

        protected int GetUserIdFromClaim()
        {
            if (int.TryParse(GetUserClaim("userid").Value, out int userId))
            {
                return userId;
            }
            else
                throw new Exception("UserId not found in claim");
        }

        private IEnumerable<Claim> GetUserClaims()
        {
            return (User.Identity as ClaimsIdentity)?.Claims;
        }

        private Claim GetUserClaim(string key)
        {
            return GetUserClaims().FirstOrDefault(c => c.Type.Equals(key, StringComparison.OrdinalIgnoreCase));
        }
    }
}
