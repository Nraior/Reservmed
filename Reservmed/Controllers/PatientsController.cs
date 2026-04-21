using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reservmed.Services.Interfaces;
using System.Security.Claims;

namespace Reservmed.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;
        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetOwnProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var data = await _patientService.GetOwnPatientProfileAsync(userId);
            if (!data.IsSuccess)
            {
                return NotFound(data.Message);
            }

            return Ok(data.Payload);
        }

    }
}
