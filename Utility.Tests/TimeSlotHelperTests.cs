using System;
using static VaccineScheduleTracking.API_Test.Helpers.TimeSlotHelper;
using System.Collections.Generic;
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
            var result = AllocateTimeSlotsAsync(input);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("2024-03-02", true)]
        [InlineData("2024-03-03", false)] // Chủ Nhật
        [InlineData("2024-03-04", true)]  // Thứ Hai
        public void ExcludedDay_CheckSundayExclusion(string date, bool expected)
        {
            var dateOnly = DateOnly.Parse(date);
            Assert.Equal(expected, ExcludedDay(dateOnly));
        }


        [Theory]
        [InlineData("07:00", 1)]
        [InlineData("07:45", 2)]
        [InlineData("08:30", 3)]
        [InlineData("09:15", 4)]
        [InlineData("10:00", 5)]
        public void CalculateSlotNumber_ReturnCorrectSlotNumber(string time, int exp)
        {
            var startTime = TimeOnly.Parse(time);
            Assert.Equal(exp, CalculateSlotNumber(startTime));
        }

        [Theory]
        [InlineData(1, "07:00")]
        [InlineData(2, "07:45")]
        [InlineData(3, "08:30")]
        [InlineData(4, "09:15")]
        public void CalculateStartTime_ReturnCorrectStartTime(int slotNumber, string exp)
        {
            Assert.Equal(TimeOnly.Parse(exp), CalculateStartTime(slotNumber));
        }



        [Theory]
        [InlineData("2024-03-04", "Monday")]
        [InlineData("2024-03-05", "Tuesday")]
        [InlineData("2024-03-07", "Thursday")]
        [InlineData("2024-03-10", "Sunday")]
        public void ConvertToWeekday_ReturnsWeekday(string dateString, string exp)
        {
            var date = DateOnly.Parse(dateString);

            Assert.Equal(exp, ConvertToWeekday(date));
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