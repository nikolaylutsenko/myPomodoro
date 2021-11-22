namespace MyPomodoro
{
    public static class MessageWriter
    {
        public static void WriteWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}