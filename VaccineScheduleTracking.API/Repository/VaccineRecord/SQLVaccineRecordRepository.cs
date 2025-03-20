using Microsoft.EntityFrameworkCore;
using VaccineScheduleTracking.API.Data;
using VaccineScheduleTracking.API_Test.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Repository.Record
{
    public class SQLVaccineRecordRepository : IVaccineRecordRepository
    {
        private readonly VaccineScheduleDbContext dbContext;

        public SQLVaccineRecordRepository(VaccineScheduleDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<VaccineRecord>?> GetRecords(int childID)
        {
            var query = dbContext.VaccineRecords.Include(x => x.Child)
                                                 .Include(x => x.VaccineType)
                                                 .Include(x => x.Vaccine)
                                                 .Include(x => x.Appointment).AsQueryable();
            return await query.Where(x => x.ChildID == childID).ToListAsync();
        }

        public async Task<VaccineRecord?> GetRecordByID(int recordID)
        {
            return await dbContext.VaccineRecords.Include(x => x.Child)
                                                 .Include(x => x.VaccineType)
                                                 .Include(x => x.Vaccine)
                                                 .OrderByDescending(c => c.VaccineRecordID)
                                                 .Include(x => x.Appointment)
                                                 .FirstOrDefaultAsync(c => c.VaccineRecordID == recordID);
        }

        public async Task<VaccineRecord> AddRecord(VaccineRecord record)
        {
            await dbContext.VaccineRecords.AddAsync(record);
            await dbContext.SaveChangesAsync();
            return record;
        }

        public async Task<VaccineRecord?> UpdateRecord(VaccineRecord record)
        {
            var updatedRecord = await dbContext.VaccineRecords.FirstOrDefaultAsync(c => c.VaccineRecordID == record.VaccineRecordID);
            if (updatedRecord != null)
            {
                updatedRecord.VaccineTypeID = record.VaccineTypeID;
                updatedRecord.VaccineID = record.VaccineID;
                updatedRecord.AppointmentID = record.AppointmentID;
                updatedRecord.Date = record.Date;
                updatedRecord.Note = record.Note;
            }
            await dbContext.SaveChangesAsync();

            return updatedRecord;
        }

        public async Task<VaccineRecord?> DeleteRecord(int recordID)
        {
            var deletedRecord = await GetRecordByID(recordID);
            if (deletedRecord != null)
            {
                dbContext.VaccineRecords.Remove(deletedRecord);
                await dbContext.SaveChangesAsync();
            }

            return deletedRecord;
        }

        public async Task<VaccineRecord?> GetRecordByAppointmentID(int appointmentId)
        {
            return await dbContext.VaccineRecords.Include(x => x.Child)
                                                 .Include(x => x.VaccineType)
                                                 .Include(x => x.Vaccine)
                                                 .Include(x => x.Appointment)
                                                 .OrderByDescending(c => c.VaccineRecordID)
                                                 .FirstOrDefaultAsync(c => c.AppointmentID == appointmentId);
        }
    }
}
