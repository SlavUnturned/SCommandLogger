namespace SCommandLogger;

public class LogFormatter
{
    [XmlAttribute]
    public string 
        Command,
        Format;
}
public partial class Config : IRocketPluginConfiguration
{
    public List<LogFormatter> Formatters = new List<LogFormatter>();
    public string
        LogType = "json",
        LogFile = "{time:dd'.'MM'.'yyyy}.log";
    public double LogSaveDelay = 15;
    public bool LogNotFormatted = false;
    public LogFormatter FindFormatter(string cmd) => Formatters.FirstOrDefault(x => 
        x.Command.Equals(cmd, StringComparison.InvariantCultureIgnoreCase) || 
        x.Command == "*");
    public void LoadDefaults()
    {
        Formatters = new()
        {
            new() { Command = "*", Format = "[{time:dd'.'MM'.'yyyy' 'hh':'mm':'ss}] {caller}[{id}] executed {name} with arguments: {0}" },
            new() { Command = "rocket", Format = "[{time:dd'.'MM'.'yyyy' 'hh':'mm':'ss}] {id} executed rocket command {1}" }
        };
    }
}
