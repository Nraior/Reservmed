using Microsoft.EntityFrameworkCore;
using Reservmed.Common;
using Reservmed.Data;
using Reservmed.DTOs;
using Reservmed.Models.Domain;
using Reservmed.Models.Identity;
using Reservmed.Services.Interfaces;

namespace Reservmed.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly ReservmedDBContext _dbContext;
        public DoctorService(ReservmedDBContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<Result> CreateDoctorAccountAsync(ApplicationUser identity, DoctorRegistrationDto dto)
        {

            var existingSpecialiations = await _dbContext.Specializations.Where(s => dto.Specializations.Contains(s.Name)).ToListAsync();

            var newAccount = new Doctor
            {
                ApplicationUserId = identity.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                BirthDate = dto.BirthDate,
                IsVerifiedByAdmin = false,
                Specializations = existingSpecialiations,
                Description = dto.Description,
                LicenseNumber = dto.LicenseNumber,
                ProfilePictureUrl = dto.ProfilePictureUrl,
            };

            try
            {
                _dbContext.Doctors.Add(newAccount);
                await _dbContext.SaveChangesAsync();
                return Result.Success("Doctor Succesffully Created");
            }
            catch (Exception ex)
            {
                return Result.Error("Failed to create Doctor Account");
            }

        }

        public async Task<bool> IsDoctorExistingAsync(string email)
        {
            return await _dbContext.Doctors.AnyAsync((doc) => doc.User.Email == email);
        }
    }
}
