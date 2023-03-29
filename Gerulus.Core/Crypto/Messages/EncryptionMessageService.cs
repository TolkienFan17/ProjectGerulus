using System.Security.Cryptography;

namespace Gerulus.Core.Crypto.Messages;

public class EncryptionMessageService : IEncryptionMessageService
{
    public required ICryptoKeyProvider KeyProvider { get; init; }

    public EncryptionMessageService(ICryptoKeyProvider keyProvider)
    {
        KeyProvider = keyProvider;
    }

    public async Task<EncryptedMessagePayload> EncryptAsync(string message, User author, User recipient)
    {
        var key = await KeyProvider.ComputeSharedKeyAsync(author.KeyPair, recipient.KeyPair);

        using var aes = Aes.Create();
        aes.GenerateIV();
        aes.Key = key;

        var encryptor = aes.CreateEncryptor();
        using (var memoryStream = new MemoryStream())
        {
            using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            using (var streamWriter = new StreamWriter(cryptoStream))
            {
                await streamWriter.WriteAsync(message);
            }

            return new EncryptedMessagePayload()
            {
                Message = memoryStream.ToArray(),
                IV = aes.IV
            };
        }
    }


    public Task<string> DecryptAsync(Message message)
        => DecryptAsync(message.GetPayload(), message.Author, message.Recipient);

    public async Task<string> DecryptAsync(EncryptedMessagePayload payload, User author, User recipient)
    {
        var key = await KeyProvider.ComputeSharedKeyAsync(author.KeyPair, recipient.KeyPair);

        using var aes = Aes.Create();
        aes.IV = payload.IV;
        aes.Key = key;

        var decryptor = aes.CreateDecryptor();

        using var memoryStream = new MemoryStream(payload.Message);
        using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        using var streamReader = new StreamReader(cryptoStream);

        return await streamReader.ReadToEndAsync();
    }

}