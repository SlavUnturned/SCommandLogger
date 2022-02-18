using System.Threading.Tasks;

namespace SCommandLogger;

public abstract class FileCommandLogger : CommandLogger
{
    readonly string filePath;
    public virtual string FilePath => filePath.FormatWith(new { Time = DateTime.Now });
    public FileCommandLogger(string filePath, TimeSpan? saveDelay = null)
    {
        this.filePath = filePath;
        Load();
        SaveDelay = saveDelay ?? TimeSpan.FromSeconds(15);
        SaveSchedule();
    }

    async void SaveSchedule() => await SaveTask();
    async Task SaveTask()
    {
        while (true)
        {
            await Task.Delay(SaveDelay);
            if (Disposed)
                return;
            Save();
        }
    }
    readonly TimeSpan SaveDelay;
    public bool Disposed { get; protected set; } = false;
    protected abstract void OnSave();
    protected virtual void OnLoad() { }
    public void Save()
    {
        if (Disposed)
            return;
        OnSave();
    }
    public void Load()
    {
        if (Disposed)
            return;
        OnLoad();
    }

    protected override void OnDispose()
    {
        if (Disposed)
            return;
        Save();
        Disposed = true;
    }
}
