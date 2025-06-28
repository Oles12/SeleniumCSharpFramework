using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EaFramework.Config;

public class ConfigReader
{
    public static TestSettings ReadConfig()
    {
        // Reads the entire contents of the appsettings.json file into a string
        var configFile = File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                                          + "/appsettings.json");

        JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };
        
        // convert string values in JSON to enum values in C#
        jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

        // Converts the JSON string into a TestSettings object using the options defined above
        return JsonSerializer.Deserialize<TestSettings>(configFile, jsonSerializerOptions);
    }
}