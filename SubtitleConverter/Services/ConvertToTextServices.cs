using System.Text.RegularExpressions;

namespace SubtitleConverter.Services
{
    public class ConvertToTextServices : IConvertToTextServices
    {
        public string ConvertVTT(string input)
        {

            var output = Regex.Replace(input, @"\d\d:\d\d:\d\d.\d\d\d --> \d\d:\d\d:\d\d.\d\d\d align:middle line:\d\d%\n\n\n", "");
            output = Regex.Replace(output, @"\d\d:\d\d:\d\d.\d\d\d --> \d\d:\d\d:\d\d.\d\d\d align:middle line:\d\d%",
                "");
            output = Regex.Replace(output, @"\r\n\r\n", " ");
            output = Regex.Replace(output, @"\r\n", " ");
            return output;
        }
    }
}