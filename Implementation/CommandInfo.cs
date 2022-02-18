namespace SCommandLogger;

public class CommandInfo : ICommandInfo
{
    public CommandInfo() { }
    public CommandInfo(string name, string[] arguments)
    {
        Name = name;
        Arguments = arguments;
    }

    public string Name { get; set; }
    public string[] Arguments { get; set; }
}
