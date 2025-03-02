using VaccineScheduleTracking.API_Test.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Repository.Record
{
    public interface IVaccineRecordRepository
    {
        Task<List<VaccineRecord>?> GetRecords(int childID);
        Task<VaccineRecord?> GetRecordByID(int recordID);
        Task<VaccineRecord> AddRecord(VaccineRecord record);
        Task<VaccineRecord?> UpdateRecord(VaccineRecord record);
        Task<VaccineRecord?> DeleteRecord(int recordID);
    }
}
