using VaccineScheduleTracking.API_Test.Models.DTOs;
using VaccineScheduleTracking.API_Test.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Services.Record
{
    public interface IVaccineRecordService 
    {
        Task<List<VaccineRecord>?> GetRecordsAsync(int childID);
        Task<VaccineRecord?> GetRecordByIDAsync(int recordID);
        Task<VaccineRecord?> UpdateVaccineRecordAsync(UpdateVaccineRecordDto updateVaccine);
        Task<VaccineRecord?> DeleteRecordAsync(int recordID);
        Task<VaccineRecord> AddVaccineHistoryAsync(ChildVaccineHistoryDto childVaccineHistory);
        Task<VaccineRecord> AddVaccineRecordAsync(CreateVaccineRecordDto createVaccineRecord);
        Task<VaccineRecord?> UpdateVaccineHistoryAsync(UpdateVaccineHistoryDto updateVaccineRecord);
        Task<VaccineRecord> GetRecordByAppointmentID(int appointmentId);
    }
}
