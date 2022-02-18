namespace SCommandLogger.API;

public interface ICommandInfo
{
    string Name { get; }
    string[] Arguments { get; }
}