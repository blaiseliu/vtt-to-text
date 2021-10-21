using CommandLine;

namespace SubtitleConverter.ParseOptions
{
    [Verb("txt", HelpText = "convert to text")]
    public class ConvertToTextOptions : ConvertOptions
    {
    }
    
    [Verb("srt", HelpText = "convert to srt")]
    public class ConvertToSrtOptions : ConvertOptions
    {
    }
    
    public abstract class ConvertOptions : ParseOptionBase
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
