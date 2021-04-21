using Serilog;
using Serilog.Core;
using SubtitleConverter.ParseOptions;

namespace SubtitleConverter.Processes
{
    public interface IProcess<T>
        where T : ParseOptionBase
    {
        void Process(T options);
    }
    public abstract class ProcessBase<T> : IProcess<T>
        where T : ParseOptionBase
    {
        public abstract void Process(T options);
        protected Logger Logger = new LoggerConfiguration()
            .WriteTo.Console()
            //.WriteTo.File($"D:\\Logs\\NetGear{DateTime.Now:yyyyMMdd.HHmmss}.log")
            .CreateLogger();
    }
}
