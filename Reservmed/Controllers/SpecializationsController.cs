using Microsoft.AspNetCore.Mvc;
using Reservmed.Services.Interfaces;

namespace Reservmed.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecializationsController : ControllerBase
    {
        ISpecializationsService _specializationService;
        public SpecializationsController(ISpecializationsService specializationService)
        {
            _specializationService = specializationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSpecializations()
        {
            var specializations = await _specializationService.GetSpecializationsAsync();
            return Ok(specializations);
        }
    }
}
