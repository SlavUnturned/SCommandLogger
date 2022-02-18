using System.Threading.Tasks;

namespace SCommandLogger;

public abstract class CommandLogger : ICommandLogger
{
    protected List<PlayerLogs> List { get; } = new();

    public virtual void Clear() => List.Clear();
    public virtual ICommandLoggerEntry Insert(ICommandLoggerEntry entry)
    {
        if (entry is not PlayerLogEntry e)
            return null;
        GetOrAdd(e.Executor).Entries.Add(e);
        return entry;
    }
    public PlayerLogs GetOrAdd(CSteamID id)
    {
        var logs = List.FirstOrDefault(x => x.Id.m_SteamID == id.m_SteamID);
        if(logs is null)
            List.Add(logs = new() { Id = id });
        return logs;
    }
    public virtual void Remove(ICommandLoggerEntry entry)
    {
        if (entry is not PlayerLogEntry e)
            return;
        GetOrAdd(entry.Executor).Entries.Remove(e);
    }
    public ICommandLoggerEntry Produce(IRocketPlayer caller, IRocketCommand command, string format, string[] args)
    {
        var fullArgs = string.Join(" ", args);
        var id = CSteamID.Nil;
        if (caller is not ConsolePlayer && ulong.TryParse(caller.Id, out var uid))
            id = new CSteamID(uid);
        var formattedMessage = format;
        args = args.Prepend(fullArgs).ToArray();
        for (int i = 0; i < args.Length; i++)
            formattedMessage = formattedMessage.Replace($"{{{i}}}", args[i]);
        formattedMessage = formattedMessage.FormatWith(new { Name = command.Name, Id = id, Caller = caller.DisplayName, Time = DateTime.Now });
        args = args.Skip(1).ToArray();
        return new PlayerLogEntry(id, new CommandInfo(command.Name, args), formattedMessage);
    }
    protected virtual void OnDispose() { }
    public void Dispose()
    {
        OnDispose();
        Clear();
    }
}
