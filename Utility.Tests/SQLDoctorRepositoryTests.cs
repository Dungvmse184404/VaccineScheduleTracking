using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using VaccineScheduleTracking.API_Test.Repository.Doctors;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API.Data;
using VaccineScheduleTracking.API_Test.Models.Entities;

public class SQLDoctorRepositoryTests
{
    private DbContextOptions<VaccineScheduleDbContext> GetDbContextOptions()
    {
        return new DbContextOptionsBuilder<VaccineScheduleDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Theory]
    [InlineData(1, "2025-03-05", 2, true)]  // tìm thấy slot
    [InlineData(2, "2025-03-06", 1, false)] // ko tìm thấy slot
    [InlineData(3, "2025-03-07", 3, false)] // ko tìm thấy slot
    public async Task GetSpecificDoctorTimeSlotAsync_Test(int doctorID, string dateStr, int slot, bool expectedResult)
    {
        var date = DateOnly.Parse(dateStr);
        var options = GetDbContextOptions();

        using (var context = new VaccineScheduleDbContext(options))
        {
            var testSchedule = new DailySchedule { AppointmentDate = DateOnly.FromDateTime(DateTime.Today) };
            var doctorTimeSlot = new DoctorTimeSlot
            {
                DoctorID = 1,
                SlotNumber = 2,
                DailySchedule = testSchedule
            };

            context.DailySchedule.Add(testSchedule);
            context.DoctorTimeSlots.Add(doctorTimeSlot);
            await context.SaveChangesAsync();
        }

        using (var context = new VaccineScheduleDbContext(options))
        {
            var repository = new SQLDoctorRepository(context);
            var result = await repository.GetSpecificDoctorTimeSlotAsync(doctorID, date, slot);

            if (expectedResult)
                Assert.NotNull(result);
            else
                Assert.Null(result);
        }
    }

    



}
