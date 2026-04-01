using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace Services
{
    public static class DataService
    {
        // EXACT OPTIONS NEEDED FOR POLYMORPHISM
        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            // This ensures the $type discriminator we added to Item.cs is used
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
            // This now uses the options to understand if an item is a Jeu or Travail
            return JsonSerializer.Deserialize<T>(json, _options);
        }
    }
}