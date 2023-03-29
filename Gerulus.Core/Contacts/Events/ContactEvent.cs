namespace Gerulus.Core.Contacts.Events;

public abstract class ContactEvent : IDomainEvent
{
    public DateTimeOffset Timestamp { get; }

    public ContactUser Recipient { get; }
    public ContactUser Sender { get; }

    public UserId IntendedRecipient => Recipient.Identity;
    public UserId IntendedSender => Sender.Identity;
    public UserId ActualSender => Sender.CoreUser;

    protected internal ContactEvent(Contact contact, DateTimeOffset? time = null)
    {
        Recipient = contact.Recipient;
        Sender = contact.Sender;
        Timestamp = time ?? DateTimeOffset.Now;
    }
}
