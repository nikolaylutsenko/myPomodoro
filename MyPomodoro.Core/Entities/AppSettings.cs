namespace MyPomodoro.Core.Entities
{
    public class AppSettings
    {
        public bool StoreInDb { get; set; }
        public bool PlaySound { get; set; }
        public int Volume { get; set; }
        public int ConcentrationTimeInMinutes { get; set; }
        public int ShortBreakInMinutes { get; set; }
        public int LongBreakInMinutes { get; set; }
    }
}