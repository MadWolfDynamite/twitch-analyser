using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Tools.StreamSerializer
{
    public static class StreamSerializer
    {
        public static T DeserialiseJsonFromStream<T>(Stream stream)
        {
            if (stream == null || stream.CanRead == false)
                return default;

            using var reader = new StreamReader(stream);
            using var json = new JsonTextReader(reader);

            var serialiser = new JsonSerializer();
            var result = serialiser.Deserialize<T>(json);

            return result;
        }
        public static async Task<string> StreamToStringAsync(Stream stream)
        {
            string content = null;

            if (stream != null)
            {
                using var reader = new StreamReader(stream);
                content = await reader.ReadToEndAsync();
            }

            return content;
        }
    }
}
