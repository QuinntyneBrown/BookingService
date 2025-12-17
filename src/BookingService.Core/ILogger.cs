namespace BookingService.Core
{
    public interface ILogger
    {
        void AddProvider(ILoggerProvider provider);
    }
}
