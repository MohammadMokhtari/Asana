using Asana.Application.Common.Interfaces;
using Asana.Application.Utilities.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Asana.WebUI.Controllers
{
    public class LocationController : ApiControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [Authorize]
        [HttpGet("GetLocation")]
        public async Task<IActionResult> GetLocation()
        {

            var result = await _locationService
                .GetLocationsAsync(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));


            return result.Succeeded ? JsonResponseStatus.Success(result.Value)
                : JsonResponseStatus.BadRequest(result.Errors);
        }

        [Authorize]
        [HttpGet("setLocation/{locationId}")]
        public async Task<IActionResult> SetDefaultLocation(long locationId)
        {

            var result = await _locationService.
                SetDefaultLocationAsync(locationId,
                Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));

            return result.Succeeded ? JsonResponseStatus.Success(result.Value)
                : JsonResponseStatus.BadRequest(result.Errors);
        }
    }
}
