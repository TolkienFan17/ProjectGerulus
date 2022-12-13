namespace Gerulus.Core.Contacts;

public class Contact
{
    public required User PrincipalUser { get; init; }
    public required User IntendedUser { get; init; }

    public User? RecipientUser { get; set; } = null;
    public ContactState IsPending { get; set; } = ContactState.Initiated;

    public byte[]? SharedSecret { get; set; }
}