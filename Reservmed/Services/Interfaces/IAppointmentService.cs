using Reservmed.Common;
using Reservmed.DTOs;

namespace Reservmed.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<Result<int>> BookAppointmentAsync(BookingAppointmentDto appointmentData);

        Task<Result<List<DateTime>>> GetAvailableAppointmentsAsync(int doctorId, DateTime endDate);

        Task<Result> UpdateAppointmentSlotsAsync(int doctorId, List<DateTime> dateSlotsToUpdate);
    }
}
