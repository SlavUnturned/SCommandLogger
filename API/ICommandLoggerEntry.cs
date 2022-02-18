namespace SCommandLogger.API;

public interface ICommandLoggerEntry
{
    ICommandInfo Command { get; set; }
    CSteamID Executor { get; set; }
    DateTime Time { get; set; }
}
