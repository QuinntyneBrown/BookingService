namespace BookingService.Core
{
    public interface ILoggerProvider
    {
        ILogger CreateLogger(string name);
    }
}
