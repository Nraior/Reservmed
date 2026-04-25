using Microsoft.EntityFrameworkCore;
using Reservmed.Common;
using Reservmed.Data;
using Reservmed.DTOs;
using Reservmed.Services.Interfaces;

namespace Reservmed.Services
{
    public class SpecializationsService : ISpecializationsService
    {
        private readonly ReservmedDBContext _dbContext;

        public SpecializationsService(ReservmedDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<IEnumerable<SpecializationDto>>> GetSpecializationsAsync()
        {
            var specializations = await _dbContext.Specializations.Select(s => new SpecializationDto { Id = s.Id, Name = s.Name }).ToListAsync();
            return Result<IEnumerable<SpecializationDto>>.Success(specializations, "Received Specializations");
        }

    }
}
