using CommandLine;

namespace SubtitleConverter.ParseOptions
{
    [Verb("convert", HelpText = "convert")]
    public class ConvertOptions : ParseOptionBase
    {
        [Option('s', "source", Required = true, HelpText = "Input file.")]
        public string Source { get; set; }
        [Option('t', "target", Required = false, HelpText = "Target file.")]
        public string Target { get; set; }
    }

    public abstract class ParseOptionBase : IParseOption
    {
    }

    public interface IParseOption
    {
    }
}
