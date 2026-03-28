using System.Text.Json;

public class DataService
{
    public static void Save<T>(string path, T data)
    {
        string json = JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(path, json);
    }

    public static T Load<T>(string path)
    {
        if (!File.Exists(path))
            return default;

        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<T>(json);
    }
}