using Newtonsoft.Json;

namespace SCommandLogger;

public class JSONCommandLogger : FileCommandLogger
{
    public JSONCommandLogger(string filePath, TimeSpan? saveDelay = null) : base(filePath, saveDelay)
    {
    }

    protected override void OnLoad()
    {
        try
        {
            if (File.Exists(FilePath))
                List.AddRange(JsonConvert.DeserializeObject<List<PlayerLogs>>(File.ReadAllText(FilePath)));
        }
        catch { }
    }

    protected override void OnSave()
    {
        File.WriteAllText(FilePath, JsonConvert.SerializeObject(List, Formatting.Indented));
    }
}