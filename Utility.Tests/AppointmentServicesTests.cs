//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using VaccineScheduleTracking.API.Models.Entities;
//using VaccineScheduleTracking.API_Test.Models.Entities;
//using VaccineScheduleTracking.API_Test.Services.Appointments;
//using Xunit;

//public class AppointmentServiceTests
//{
//    private readonly AppointmentService _appointmentService;

//    public AppointmentServiceTests()
//    {
//        _appointmentService = new AppointmentService();
//    }

//    private static Appointment CreateTestAppointment(int id, int accountId, int childId, int vaccineId, int timeSlotId, string status)
//    {
//        return new Appointment
//        {
//            AppointmentID = id,
//            AccountID = accountId,
//            Account = new Account
//            {
//                AccountID = accountId,
//                Firstname = "John",
//                Lastname = "Doe",
//                Email = "john.doe@example.com",
//                PhoneNumber = "123456789",
//                Status = "Active"
//            },
//            ChildID = childId,
//            Child = new Child
//            {
//                ChildID = childId,
//                Firstname = "Baby",
//                Lastname = "Doe",
//                Gender = "Male",
//                DateOfBirth = new DateOnly(2020, 1, 1),
//                ParentID = accountId,
//                Parent = new Parent()
//            },
//            VaccineID = vaccineId,
//            Vaccine = new Vaccine
//            {
//                VaccineID = vaccineId,
//                Name = "MMR",
//                Manufacturer = "XYZ Pharma",
//                Stock = 10,
//                Price = 100.0m,
//                FromAge = 1,
//                ToAge = 10
//            },
//            TimeSlotID = timeSlotId,
//            TimeSlots = new TimeSlot
//            {
//                TimeSlotID = timeSlotId,
//                StartTime = new TimeOnly(9, 0),
//            },
//            Status = status
//        };
//    }

//    // Test CancelAppointmentAsync
//    [Theory]
//    [InlineData(1, true)]   // Hủy thành công
//    [InlineData(999, false)] // Hủy thất bại (ID không tồn tại)
//    public async Task CancelAppointmentAsync_ShouldReturnExpectedResult(int appointmentId, bool expectedResult)
//    {
//        var result = await _appointmentService.CancelAppointmentAsync(appointmentId);
//        Assert.Equal(expectedResult, result);
//    }

//    // Test CreateAppointmentAsync
//    [Fact]
//    public async Task CreateAppointmentAsync_ShouldCreateSuccessfully()
//    {
//        var appointment = CreateTestAppointment(3, 1, 2, 1, 1, "Pending");
//        var result = await _appointmentService.CreateAppointmentAsync(appointment);
//        Assert.True(result);
//    }

//    // Test UpdateAppointmentAsync
//    [Fact]
//    public async Task UpdateAppointmentAsync_ShouldUpdateSuccessfully()
//    {
//        var appointment = CreateTestAppointment(1, 1, 2, 1, 1, "Confirmed");
//        var result = await _appointmentService.UpdateAppointmentAsync(appointment);
//        Assert.True(result);
//    }

//    // Test GetAppointmentListByIDAsync
//    [Theory]
//    [InlineData(1, 2)]   // User có 2 lịch hẹn
//    [InlineData(999, 0)] // User không có lịch hẹn
//    public async Task GetAppointmentListByIDAsync_ShouldReturnExpectedCount(int userId, int expectedCount)
//    {
//        var result = await _appointmentService.GetAppointmentListByIDAsync(userId);
//        Assert.Equal(expectedCount, result.Count);
//    }
//}
