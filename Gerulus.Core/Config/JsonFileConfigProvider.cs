using System.Text.Json;

namespace Gerulus.Core.Config;

public class JsonFileConfigProvider : IConfigProvider
{
    private const string ConfigFileLocation = "gerulus.config";
    private AppConfiguration? Config { get; set; }

    public async Task LoadAsync()
    {
        var configText = await File.ReadAllTextAsync(ConfigFileLocation);
        Config = JsonSerializer.Deserialize<AppConfiguration>(configText);
    }

    public AppConfiguration Get()
    {
        if (Config is null) throw new InvalidOperationException("Cannot retrieve the config because it has not been loaded in");

        return Config;
    }

    public Task SaveAsync()
    {
        var configText = JsonSerializer.Serialize(Config);
        return File.WriteAllTextAsync(ConfigFileLocation, configText);
    }
}