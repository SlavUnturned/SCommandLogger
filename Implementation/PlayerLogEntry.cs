using Newtonsoft.Json;

namespace SCommandLogger;

public class PlayerLogEntry : ICommandLoggerEntry
{
    public PlayerLogEntry() { }
    public PlayerLogEntry(CSteamID executor, ICommandInfo command, string formatted)
    {
        Executor = executor;
        (this as ICommandLoggerEntry).Command = command;
        Formatted = formatted;
    }
    public string Formatted { get; set; }
    [JsonIgnore, XmlIgnore]
    ICommandInfo ICommandLoggerEntry.Command { get => Command; set => Command = value as CommandInfo; }
    [JsonProperty, XmlElement]
    CommandInfo Command { get; set; }
    [JsonIgnore, XmlIgnore]
    public CSteamID Executor { get; set; }
    public DateTime Time { get; set; } = DateTime.Now;
    public override string ToString() => Formatted;
}
