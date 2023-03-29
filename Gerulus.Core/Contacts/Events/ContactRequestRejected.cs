namespace Gerulus.Core.Contacts.Events;

public class ContactRequestRejected : ContactEvent
{
    internal ContactRequestRejected(Contact contact, DateTimeOffset? time = null) : base(contact, time)
    {
    }
}
