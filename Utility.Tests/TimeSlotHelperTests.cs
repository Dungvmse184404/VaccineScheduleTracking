using System;
using System.Collections.Generic;
using VaccineScheduleTracking.API_Test.Helpers;
using Xunit;


namespace Utility.Tests
{
    public class TimeSlotHelperTests
    {
        [Theory]
        [InlineData("1,2,3,4,5", new int[] { 1, 2, 3, 4, 5 })]
        [InlineData(null, new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 })]
        public void AllocateTimeSlotsAsync_ValidInput_ReturnsList(string input, int[] expected)
        {
            var result = TimeSlotHelper.AllocateTimeSlotsAsync(input);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("2024-03-03", false)] // Chủ Nhật
        [InlineData("2024-03-04", true)]  // Thứ Hai
        public void ExcludedDay_CheckSundayExclusion(string date, bool expected)
        {
            var dateOnly = DateOnly.Parse(date);
            var result = TimeSlotHelper.ExcludedDay(dateOnly);

            Assert.Equal(expected, result);
        }


        [Theory]
        [InlineData("07:00", 1)]
        [InlineData("07:45", 2)]
        [InlineData("08:30", 3)]
        [InlineData("09:15", 4)]
        [InlineData("10:00", 5)]
        public void CalculateSlotNumber_ReturnCorrectSlotNumber(string time, int expectedSlotNumber)
        {
            var startTime = TimeOnly.Parse(time);
            var slotNumber = TimeSlotHelper.CalculateSlotNumber(startTime);

            Assert.Equal(expectedSlotNumber, slotNumber);
        }

        [Theory]
        [InlineData(1, "07:00")]
        [InlineData(2, "07:45")]
        [InlineData(3, "08:30")]
        [InlineData(4, "09:15")]
        public void CalculateStartTime_ShouldReturnCorrectStartTime(int slotNumber, string expectedTime)
        {
            var startTime = TimeSlotHelper.CalculateStartTime(slotNumber);

            Assert.Equal(TimeOnly.Parse(expectedTime), startTime);
        }




        [Fact]
        public void ConvertToWeekday_ValidDate_ReturnsWeekday()
        {
            var date = new DateOnly(2024, 3, 4); // Thứ Hai
            var result = TimeSlotHelper.ConvertToWeekday(date);

            Assert.Equal("Monday", result);
        }



        //[Fact]
        //public void LimitDate_DateExceedsLimit_ThrowsException()
        //{
        //    var futureDate = TimeSlotHelper.CaculateDate(TimeSlotHelper.SetCalanderDate() + 1);
        //    string expectedMessage = $"(BE)err: Lịch không hợp lệ {futureDate}";

        //    var exception = Assert.Throws<Exception>(() => TimeSlotHelper.LimitDate(futureDate, "Lịch không hợp lệ"));
        //    Assert.Equal(expectedMessage, exception.Message);
        //}
    }


}