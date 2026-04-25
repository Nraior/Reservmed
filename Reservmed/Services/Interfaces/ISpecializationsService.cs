using Reservmed.Common;
using Reservmed.DTOs;

namespace Reservmed.Services.Interfaces
{
    public interface ISpecializationsService
    {
        public Task<Result<IEnumerable<SpecializationDto>>> GetSpecializationsAsync();
    }
}
