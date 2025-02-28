using Microsoft.AspNetCore.Mvc;

namespace VaccineScheduleTracking.API_Test.Helpers
{
    public static class ExceptionHelper
    {
        public static IActionResult HandleException(Exception ex)
        {
            var errorMessage = ex.Message;
            if (ex.InnerException != null)
            {
                errorMessage += $"\nInner Exception: {ex.InnerException.Message}";
            }
            return new BadRequestObjectResult(new
            {
                Message = errorMessage
            });
        }

        public static void WriteLog(string message)
        {
            string logFilePath = @"E:\Backup\SWP391\VaccineScheduleTracking_Test\Log.txt";
            string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}{Environment.NewLine}";

            try
            {
                File.AppendAllText(logFilePath, logMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi ghi log: {ex.Message}");
            }
        }

    }
}
