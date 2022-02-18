using System.Xml.Serialization;
using Newtonsoft.Json;

namespace SCommandLogger;

public class CommandInfo : ICommandInfo
{
    public CommandInfo() { }
    public CommandInfo(string name, string[] arguments)
    {
        Name = name;
        Arguments = arguments;
    }

    [XmlAttribute]
    public string Name { get; set; }
    [XmlArrayItem("Argument")]
    public string[] Arguments { get; set; }
}
