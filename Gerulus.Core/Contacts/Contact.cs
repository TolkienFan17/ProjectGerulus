using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gerulus.Core.Contacts;

public class Contact
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; init; }

    public required User ActualSender { get; init; }
    public User? IntendedSender { get; set; } = null;

    public required User IntendedRecipient { get; init; }
    public User? ActualRecipient { get; set; } = null;

    public bool IsPending { get; set; } = true;

    public byte[]? SharedSecret { get; set; }
}