using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace Services
{
    public static class DataService
    {
        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public static void Save<T>(string filePath, T data)
        {
            string json = JsonSerializer.Serialize(data, _options);
            File.WriteAllText(filePath, json);
        }

        public static T Load<T>(string filePath)
        {
            if (!File.Exists(filePath)) return default;

            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(json, _options);
        }
    }
}