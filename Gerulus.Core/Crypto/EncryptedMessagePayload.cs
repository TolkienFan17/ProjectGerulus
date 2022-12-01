namespace Gerulus.Core.Crypto;

public struct EncryptedMessagePayload
{
    public required byte[] Message { get; init; }
    public required byte[] IV { get; init; }
}