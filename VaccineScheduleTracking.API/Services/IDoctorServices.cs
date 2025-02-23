using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.Entities;

namespace VaccineScheduleTracking.API.Services
{
    public interface IDoctorServices
    {
        Task GenerateDoctorCalanderAsync(List<Doctor> doctorList, int numberOfDays);
        Task<Doctor> GetSutableDoctorAsync(int slotNumber ,DateOnly date);

        Task UpdateDoctorScheduleAsync(DoctorTimeSlot doctorSlot);

        //Task<Doctor> GetSuitableDoctor(DateOnly date, int SlotNumber);
    }
}
