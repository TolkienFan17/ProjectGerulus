using System.Text.Json.Serialization;

namespace Gerulus.Core.Config;

public class AppConfiguration
{
    [JsonPropertyName("databaseFileLocation")]
    public string DatabaseFileLocation { get; set; } = "gerulus.db";

    [JsonPropertyName("encryptionParametersFileLocation")]
    public string ParametersFileLocation { get; set; } = "encryption-parameters.pem";
}
