using System.IO;
using Microsoft.Extensions.Logging;
using SubtitleConverter.ParseOptions;
using SubtitleConverter.Services;

namespace SubtitleConverter.Processes
{
    public class ConvertProcess : IConvertProcess
    {
        private readonly ILogger<ConvertProcess> _logger;
        private readonly IConvertServices _services;

        public ConvertProcess(ILogger<ConvertProcess> logger, IConvertServices services)
        {
            _logger = logger;
            _services = services;
        }

        public void Process(ConvertOptions options)
        {
            var source = options.Source;
            var target = string.IsNullOrEmpty(options.Target) ? $"{source}.txt" : options.Target;

            _logger.LogInformation($"Source: {source}");
            _logger.LogInformation($"Target: {target}");

            using var sr = new StreamReader(source);
            var input = sr.ReadToEnd();
            var output = _services.ConvertVTT(input);
            using var sw = new StreamWriter(target);
            sw.WriteLine(output);
            _logger.LogInformation("Completed.");
        }

    }
}