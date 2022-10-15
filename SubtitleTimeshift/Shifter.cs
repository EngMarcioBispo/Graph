using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubtitleTimeshift
{
    public class Shifter
    {
        async static public Task Shift(Stream input, Stream output, TimeSpan timeSpan, Encoding encoding, int bufferSize = 1024, bool leaveOpen = false)
        {
            try {
                using (var read = new StreamReader(input, encoding, false, bufferSize, leaveOpen))
                using (var write = new StreamWriter(output, encoding, bufferSize, leaveOpen))
                {
                    string row;
                    var spacer = " --> ";
                    while ((row = await read.ReadLineAsync()) != null)
                    {
                        var outputLine = row;
                        var stamps = row.Split(new[] {spacer}, StringSplitOptions.RemoveEmptyEntries);
                        if (stamps.Length == 2)
                        {
                            var start = shiftStamp(stamps[0], timeSpan);
                            var end = shiftStamp(stamps[1], timeSpan);
                            outputLine = $"{start}{spacer}{end}";
                        }
                        await write.WriteLineAsync(outputLine);
                    }
                }
            }
            catch (Exception e) {                
                throw e;
            }
        }
        static string shiftStamp(string stampText, TimeSpan timeSpan)
        {
            var numbers = stampText.Split(new[]{ ':', ',', '.', '|' }).Select(s => int.Parse(s)).ToArray();
            var result = new TimeSpan(0, numbers[0], numbers[1], numbers[2], numbers[3]).Add(timeSpan).ToString(@"hh\:mm\:ss\.fff");
            return result;
        }
    }
}
