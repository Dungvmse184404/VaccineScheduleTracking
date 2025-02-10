namespace VaccineScheduleTracking.API.Helpers
{
    public class MessageColorUtility
    {

        public enum Color
        {
            Red = 1,
            Green = 2,
            Yellow = 3,
            Blue = 4,
            Magenta = 5,
            Cyan = 6,
            White = 7
        }
        public static void DisplayMsg(string message, Color color)
        {
            // Lấy mã màu ANSI
            string colorCode = color switch
            {
                Color.Red => "\x1b[31m", // Red
                Color.Green => "\x1b[32m", // Green
                Color.Yellow => "\x1b[33m", // Yellow
                Color.Blue => "\x1b[34m", // Blue
                Color.Magenta => "\x1b[35m", // Magenta
                Color.Cyan => "\x1b[36m", // Cyan
                Color.White => "\x1b[37m", // White
                _ => "\x1b[0m", // Default (reset color)
            };

            // In thông báo với màu
            Console.WriteLine($"{colorCode}{message}\x1b[0m");
        }


    }
}
