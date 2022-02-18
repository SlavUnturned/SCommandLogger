namespace SCommandLogger;

public class PlayerLogs
{
    public CSteamID Id { get; set; }
    public List<PlayerLogEntry> Entries { get; set; } = new();
    public override string ToString() => string.Join(NewLine, Entries.AsLines())+NewLine;
}
