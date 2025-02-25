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
    }
}
