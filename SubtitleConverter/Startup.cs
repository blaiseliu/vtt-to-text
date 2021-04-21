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
        private readonly IConvertProcess _process;

        public Startup(ILogger<Startup> log, IConvertProcess process)
        {
            _log = log;
            _process = process;
        }
        #endregion
        public void Run(string[] args)
        {
            Parser.Default.ParseArguments<ConvertOptions>(args)
                .WithParsed(x => _process.Process(x))
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