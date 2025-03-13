using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Services.Doctors
{
    public interface IDoctorServices
    {
        Task<List<Account>> GetAllDoctorAsync();
        Task<DoctorTimeSlot?> GetSuitableDoctorTimeSlotAsync(int slotNumber, DateOnly date, int? exclusiveID = null);
        Task<Account?> GetDoctorByIDAsync(int doctorID);
        Task<List<Account>> GetDoctorByTimeSlotAsync(int slotNumber, DateOnly date);
        Task<Account?> UpdateDoctorAsync(int doctorId, string doctorSchedule);
        Task<List<Appointment>> ReassignDoctorAppointmentsAsync(int doctorId, List<Appointment> appointments);
        Task<Account> AddDoctorByAccountIdAsync(Account account, string doctorSchedule);

        //----------------------------Doctor Schedule---------------------------
        Task<Doctor?> ManageDoctorScheduleServiceAsync(int doctorId, string doctorSchedule);
        Task<DoctorTimeSlot?> FindDoctorTimeSlotAsync(int doctorId, DateOnly date, int slotNumber);
        Task<DoctorTimeSlot> SetDoctorTimeSlotAsync(DoctorTimeSlot doctorTimeSlot, bool status);
        Task<DoctorTimeSlot> UpdateDoctorTimeSlotAsync(DoctorTimeSlot doctorSlot);
        Task SetOverdueDoctorScheduleAsync();
        Task GenerateDoctorCalanderAsync(List<Doctor> doctorList, int numberOfDays);
        Task DeleteDoctorTimeSlotAsync(int doctorId);


    }
}
