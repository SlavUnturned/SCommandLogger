namespace SCommandLogger.API;

public interface ICommandLogger : IDisposable
{
    ICommandLoggerEntry Produce(IRocketPlayer caller, IRocketCommand command, string format, string[] args);
    ICommandLoggerEntry Insert(ICommandLoggerEntry entry);
    void Remove(ICommandLoggerEntry entry);
    void Clear();
}
