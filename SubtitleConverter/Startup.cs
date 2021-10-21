using System;
using System.Collections.Generic;
using CommandLine;
using Microsoft.Extensions.Logging;
using SubtitleConverter.ParseOptions;
using SubtitleConverter.Processes;

namespace SubtitleConverter
{
    internal class Startup
    {
        #region DI
        private readonly ILogger<Startup> _log;
        private readonly IConvertToTextProcess _convertToText;
        private readonly IConvertToSrtProcess _convertToSrt;

        public Startup(ILogger<Startup> log, IConvertToSrtProcess convertToSrt, IConvertToTextProcess convertToText)
        {
            _log = log;
            _convertToSrt = convertToSrt;
            _convertToText = convertToText;
        }
        #endregion
        public void Run(string[] args)
        {
            Parser.Default.ParseArguments<ConvertToTextOptions, ConvertToSrtOptions>(args)
                .WithParsed<ConvertToTextOptions>(x => _convertToText.Process(x))
                .WithParsed<ConvertToSrtOptions>(x => _convertToSrt.Process(x))
                .WithNotParsed(HandleParseError);

            Console.WriteLine("Press any key to finish.");
            Console.ReadKey(true);
        }

        private static void HandleParseError(IEnumerable<Error> errors)
        {
            foreach (var error in errors)
            {
                Console.WriteLine(error);
            }
        }
    }
}