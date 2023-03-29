namespace Gerulus.Core.Crypto.Messages;

public readonly struct EncryptedMessagePayload
{
    public required byte[] Message { get; init; }
    public required byte[] IV { get; init; }
}