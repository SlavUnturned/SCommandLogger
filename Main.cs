using System.Linq.Expressions;
using HarmonyLib;
using Rocket.Core.Commands;
using SDG.Unturned;

namespace SCommandLogger;

[Harmony]
public sealed class Main : RocketPlugin<Config>
{
    public static Main Instance { get; private set; }
    public Harmony Harmony { get; private set; }
    static ICommandLogger logger;
    public static ICommandLogger Logger
    {
        get => logger;
        set
        {
            logger?.Dispose();
            logger = value;
        }
    }

    protected override void Unload()
    {
        Harmony?.UnpatchAll(Harmony.Id);
        Harmony = null;
        Logger = null;
        Instance = null;
    }
    protected override void Load()
    {
        Instance = this;
        var saveDelay = TimeSpan.FromSeconds(conf.LogSaveDelay);
        Logger = conf.LogType.ToLower() switch
        {
            "json" => new JSONCommandLogger(conf.LogFile, saveDelay),
            _ => new TextCommandLogger(conf.LogFile, saveDelay)
        };
        Harmony = new Harmony(nameof(SCommandLogger));
        Harmony.PatchAll();
    }

    [HarmonyPatch(typeof(RocketCommandManager), nameof(Execute)), HarmonyTranspiler]
    static IEnumerable<CodeInstruction> ExecutePatch(IEnumerable<CodeInstruction> instrs)
    {
        var list = instrs.ToList();
        var idx = list.FindIndex(x => x.Calls(typeof(IRocketCommand).GetMethod(nameof(Execute))));
        if (idx < 0)
            return list;
        list[idx] = CodeInstruction.Call(typeof(Main), nameof(Execute));
        return list;
    }
    static void ExecuteHandler(IRocketCommand command, IRocketPlayer player, string[] args)
    {
        try
        {
            if (Instance is null)
                return;
            var formatter = conf.FindFormatter(command.Name);
            if (conf.LogNotFormatted || formatter is not null)
            {
                formatter ??= new();
                CLogger.Insert(CLogger.Produce(player, command, formatter.Format, args));
            }
        }
#if DEBUG
        catch (Exception ex) { Rocket.Core.Logging.Logger.LogException(ex); }
#else
        catch { }
#endif
    }
    public static void Execute(IRocketCommand command, IRocketPlayer player, string[] args)
    {
        if (command is null)
            return;
        ExecuteHandler(command, player, args);
        command.Execute(player, args);
    }

    #region Translations
    public override TranslationList DefaultTranslations => DefaultTranslationList;

    public new string Translate(string key, params object[] args) => base.Translate(key.Trim(TranslationKeyTrimCharacters), args);
    #endregion
}
