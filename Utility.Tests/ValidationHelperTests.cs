using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineScheduleTracking.API_Test.Helpers;

namespace Utility.Tests
{
    public class ValidationHelperTests
    {
        [Theory]
        [InlineData(null, false)]  // Kiểm tra null
        [InlineData("", true)]    // Chuỗi rỗng - ko chửi
        [InlineData("string", false)]  // "string" - chửi
        [InlineData(0, false)]     // Số 0 - chửi
        [InlineData(5, true)]      // khác 0 - ko chửi
        [InlineData(0.0, false)]   // Số thực 0 - chửi
        [InlineData(3.14, true)]   // Số thực khác 0 - ko chửi
        public void NullValidator_ValidateCorrectly(object value, bool expected)
        {
            var result = ValidationHelper.NullValidator(value);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ValidateInput_ThrowException_WhenInputIsDefault()
        {
            // Arrange
            int defaultValue = default;
            string errorMessage = "Input cannot be default";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => ValidationHelper.ValidateInput(defaultValue, errorMessage));
            Assert.Equal(errorMessage, exception.Message);
        }

        [Fact]
        public void ValidateInput_ShouldNotThrowException_WhenInputIsValid()
        {
            // Arrange
            int validValue = 5;
            string errorMessage = "Input cannot be default";

            // Act & Assert
            Exception ex = Record.Exception(() => ValidationHelper.ValidateInput(validValue, errorMessage));
            Assert.Null(ex); // Không có ngoại lệ nào xảy ra
        }
    }
}
