namespace Gerulus.Core.Crypto;

public interface IEncryptionMessageService
{
    Task<EncryptedMessagePayload> EncryptAsync(string message, User author, User recipient);
    Task<string> DecryptAsync(Message message);
    Task<string> DecryptAsync(EncryptedMessagePayload payload, User author, User recipient);
}
