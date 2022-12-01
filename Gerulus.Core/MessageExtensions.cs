using Gerulus.Core.Crypto;

namespace Gerulus.Core;

public static class MessageExtensions
{
    public static EncryptedMessagePayload GetPayload(this Message message)
    {
        return new EncryptedMessagePayload()
        {
            Message = message.Contents,
            IV = message.IV
        };
    }
}