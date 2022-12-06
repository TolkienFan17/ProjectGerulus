using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gerulus.Core;

public class Message
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; init; }

    public required User Author { get; init; }
    public required User Recipient { get; init; }
    public required string Title { get; init; }
    public required byte[] Contents { get; init; }
    public required byte[] IV { get; init; }
    public DateTimeOffset SentTime { get; }
    public bool IsRead { get; set; } = false;

    private Message()
    {
    }

    public Message(DateTimeOffset? sentTime = null)
    {
        SentTime = sentTime ?? DateTimeOffset.Now;
    }
}