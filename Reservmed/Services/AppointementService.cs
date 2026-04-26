using Reservmed.Common;
using Reservmed.DTOs;
using Reservmed.Services.Interfaces;

namespace Reservmed.Services
{
    public class AppointementService : IAppointmentService
    {
        public Task<Result<int>> BookAppointmentAsync(BookingAppointmentDto appointmentData)
        {
            throw new NotImplementedException();
        }

        public Task<Result<List<DateTime>>> GetAvailableAppointmentsAsync(int doctorId, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateAppointmentSlotsAsync(int doctorId, List<DateTime> dateSlotsToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
