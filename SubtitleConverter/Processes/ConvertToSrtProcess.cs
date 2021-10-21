using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using SubtitleConverter.ParseOptions;
using SubtitleConverter.Services;

namespace SubtitleConverter.Processes
{
    public class ConvertToSrtProcess : IConvertToSrtProcess
    {
        static string subripFileExtension = ".srt";
        static string simpleTimeFormat = "mm:ss,fff";
        static string extendedTimeFormat = "HH:mm:ss,fff";

        #region DI
        private readonly ILogger<ConvertToSrtProcess> _logger;
        private readonly IConvertToSrtServices _services;

        public ConvertToSrtProcess(ILogger<ConvertToSrtProcess> logger, IConvertToSrtServices services)
        {
            _logger = logger;
            _services = services;
        }
        #endregion

        public void Process(ConvertOptions options)
        {
            var source = options.Source;
            var target = string.IsNullOrEmpty(options.Target) ? $"{source}{subripFileExtension}" : options.Target;

            _logger.LogInformation($"Source: {source}");
            _logger.LogInformation($"Target: {target}");

            //var regexTimeStamp = new Regex(@"\d\d:\d\d:\d\d.\d\d\d --> \d\d:\d\d:\d\d.\d\d\d");

            var output = new StringBuilder();
            using var sr = new StreamReader(source);

            int lineNumber = 1;
            var previousLine = "";
            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();

                if (IsTimecode(line))
                {
                    output.AppendLine();
                    output.AppendLine(lineNumber.ToString());
                    _logger.LogInformation($"Line: {lineNumber}");
                    lineNumber++;

                    line = line.Replace('.', ',');
                    line = Regex.Replace(line, @"align:\b\w*\b", String.Empty);
                    line = Regex.Replace(line, @"position:\d*%", String.Empty);
                    _logger.LogInformation($"Line: {line}");

                    string timeSrt1 = line.Substring(0, line.IndexOf('-'));
                    string timeSrt2 = line.Substring(line.IndexOf('>') + 1);
                    int divIt1 = timeSrt1.Count(x => x == ':');
                    int divIt2 = timeSrt1.Count(x => x == ':');

                    string timeFormat = simpleTimeFormat;
                    if (divIt1 != divIt2)
                        throw new Exception("Invalid Time Format");

                    if (divIt1 == 2 && divIt2 == 2)
                        timeFormat = extendedTimeFormat;

                    output.AppendLine(line);
                }

                else
                {
                    _logger.LogInformation($"prev: {previousLine}");
                    _logger.LogInformation($"line: {line}");
                    line = DeleteCueSettings(line);

                    if (!string.IsNullOrWhiteSpace(line) &&
                        line != previousLine)
                    {
                        output.AppendLine(line);
                        previousLine = line;
                    }
                }
            }


            using var sw = new StreamWriter(target);
            sw.WriteLine(output);
            _logger.LogInformation("Completed.");
            _logger.LogInformation($"Location: {target}");
        }


        bool IsTimecode(string line)
        {
            return line.Contains("-->");
        }

        string DeleteCueSettings(string line)
        {
            //StringBuilder output = new StringBuilder();
            //foreach (char ch in line)
            //{
            //    char chLower = Char.ToLower(ch);
            //    if (chLower >= 'a' && chLower <= 'z')
            //    {
            //        break;
            //    }
            //    output.Append(ch);
            //}
            //return output.ToString();
            //Regex regex = new Regex(@"</?\w+((\s+\w+(\s*=\s*(?:"".*?""|'.*?'|[^'"">\s]+))?)+\s*|\s*)/?>", RegexOptions.Singleline);
            line = Regex.Replace(line, @"<\d\d:\d\d:\d\d\.\d\d\d\>", string.Empty);
            line = Regex.Replace(line, @"<c>", string.Empty);
            line = Regex.Replace(line, @"</c>", string.Empty);
            line = Regex.Replace(line, @"</?\w+((\s+\w+(\s*=\s*(?:"".*?""|'.*?'|[^'"">\s]+))?)+\s*|\s*)/?>", String.Empty);

            return line;
        }


    }
}