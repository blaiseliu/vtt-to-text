namespace SubtitleConverter.Services
{
    public interface IConvertToTextServices
    {
        string ConvertVTT(string input);
    }
    public interface IConvertToSrtServices
    {
        string ConvertVTT(string input);
    }
}