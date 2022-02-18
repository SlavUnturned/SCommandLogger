﻿global using Rocket.API;
global using Rocket.API.Collections;
global using Rocket.API.Serialisation;
global using Rocket.Core;
global using Rocket.Core.Assets;
global using Rocket.Core.Logging;
global using Rocket.Core.Plugins;
global using Rocket.Unturned;
global using Rocket.Unturned.Chat;
global using Rocket.Unturned.Events;
global using Rocket.Unturned.Player;
global using SDG.Unturned;
global using Steamworks;
global using System;
global using System.Collections;
global using System.Collections.Generic;
global using System.IO;
global using System.Linq;
global using System.Reflection;
global using System.Text;
global using System.Text.RegularExpressions;
global using System.Threading;
global using System.Xml.Serialization;
global using UnityEngine;
global using static SCommandLogger.Utils;
global using IRP = Rocket.API.IRocketPlayer;
global using Logger = Rocket.Core.Logging.Logger;
global using UP = Rocket.Unturned.Player.UnturnedPlayer;
global using V = SDG.Unturned.InteractableVehicle;
global using P = SDG.Unturned.Player;
global using SP = SDG.Unturned.SteamPlayer;
global using Color = UnityEngine.Color;
global using SCommandLogger.API;
using System.ComponentModel;
using System.Web.UI;

namespace SCommandLogger;

public static partial class Utils
{
    public static Main inst => Main.Instance;
    public static Config conf => inst.Configuration.Instance;
    public static ICommandLogger CLogger => Main.Logger;

    public static readonly string NewLine = "\n\r";
    public static IEnumerable<string> AsLines(this IEnumerable<object> strings) => strings.Select(x => x.ToString()).Where(x => !string.IsNullOrWhiteSpace(x));
    public static string FormatWith(this string format, object source)
    {
        return FormatWith(format, null, source);
    }
    public static string FormatWith(this string format, IFormatProvider provider, object source)
    {
        if (format == null)
            throw new ArgumentNullException("format");

        Regex r = new Regex(@"(?<start>\{)+(?<property>[\w\.\[\]]+)(?<format>:[^}]+)?(?<end>\})+",
          RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

        List<object> values = new List<object>();
        string rewrittenFormat = r.Replace(format, delegate (Match m)
        {
            Group startGroup = m.Groups["start"];
            Group propertyGroup = m.Groups["property"];
            Group formatGroup = m.Groups["format"];
            Group endGroup = m.Groups["end"];

            values.Add((propertyGroup.Value == "0")
              ? source
              : DataBinder.Eval(source, propertyGroup.Value));

            return new string('{', startGroup.Captures.Count) + (values.Count - 1) + formatGroup.Value
              + new string('}', endGroup.Captures.Count);
        });

        return string.Format(provider, rewrittenFormat, values.ToArray());
    }
}