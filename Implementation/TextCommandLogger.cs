using Newtonsoft.Json;

namespace SCommandLogger;

public class TextCommandLogger : FileCommandLogger
{
    public TextCommandLogger(string filePath, TimeSpan? saveDelay = null) : base(filePath, saveDelay)
    {
    }

    protected override void OnLoad() { }

    protected override void OnSave()
    {
        File.AppendAllLines(FilePath, List.SelectMany(x => x.Entries.AsLines()));
        Clear();
    }
}