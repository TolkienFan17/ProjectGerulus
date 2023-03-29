namespace Gerulus.Core.Contacts.Events;

public class ContactRequestSent : ContactEvent
{
    internal ContactRequestSent(Contact contact, DateTimeOffset? time = null) : base(contact, time)
    {
    }
}
