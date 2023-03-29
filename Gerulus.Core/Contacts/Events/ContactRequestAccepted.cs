namespace Gerulus.Core.Contacts.Events;

public class ContactRequestAccepted : ContactEvent
{
    public UserId ActualRecipient => Recipient.CoreUser;

    internal ContactRequestAccepted(Contact contact, DateTimeOffset? time = null) : base(contact, time)
    {
    }
}
