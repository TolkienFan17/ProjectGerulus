using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gerulus.Core.Contacts;

public class Contact
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; init; }

    public required User PrincipalUser { get; init; }
    public required User IntendedUser { get; init; }

    public User? RecipientUser { get; set; } = null;
    public ContactState State { get; set; } = ContactState.Initiated;

    public byte[]? SharedSecret { get; set; }
}