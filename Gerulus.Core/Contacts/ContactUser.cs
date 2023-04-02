using System.Collections.Immutable;
using Gerulus.Core.Contacts.Events;

namespace Gerulus.Core.Contacts;

public class ContactUser : AggregateRoot<ContactUserId>
{
    private HashSet<Contact> ContactsList { get; } = new();
    public ImmutableList<Contact> Contacts => ContactsList.ToImmutableList();

    public UserId CoreUser => Id.Core;
    public UserId Identity => Id.Identity;

    public ContactUser(UserId user) : this(user, user)
    {
    }

    public ContactUser(UserId actualUser, UserId identityUser) 
        : base(new ContactUserId(actualUser, identityUser))
    {
    }

    public ContactUser? GetContactWith(ContactUser user)
    {
        // TODO Refactor this
        var contact = ContactsList.FirstOrDefault(contact => contact.Recipient.Identity.Equals(user.Identity));
        if (contact is not null)
            return contact.Recipient;

        contact = ContactsList.FirstOrDefault(contact => contact.Sender.Identity.Equals(user.Identity));
        if (contact is not null)
            return contact.Sender;

        return null;
    }

    public void RequestContactWith(ContactUser user)
    {
        CreateContact(user);
    }

    public ContactUser Impersonate(ContactUser victim)
    {
        return new ContactUser(CoreUser, victim.Identity);
    }

    private void CreateContact(ContactUser recipient)
    {
        if (ContactsList.Any(contact => contact.IsIdenticalTo(this, recipient)))
        {
            // Business logic:
            // If the user has rejected the request, the sender must remain ignorant. Therefore, do nothing.
            // If the user has accepted the request, the request is unnecessary, and this method is idempotent. Therefore, do nothing.
            // If the user has neither accepted nor rejected the request, then there is nothing to do other than wait. Therefore, do nothing.
        }

        var contact = new Contact(this, recipient);
        ContactsList.Add(contact);
        recipient.ReceiveContactUpdate(contact);

        // Raise Domain Event
        Events.Add(new ContactRequestSent(contact));
    }

    public void Accept(ContactUser sender)
    {
        var contact = ContactsList.Single(contact => contact.Sender.Equals(sender) && contact.Recipient.Identity.Equals(Identity));
        contact.Accept(this);
        contact.Recipient.ReceiveContactUpdate(contact);

        Events.Add(new ContactRequestAccepted(contact));
    }

    public void Reject(ContactUser sender)
    {
        var contact = ContactsList.Single(contact => contact.Sender.Equals(sender) && contact.Recipient.Identity.Equals(Identity));
        contact.Reject();
        contact.Recipient.ReceiveContactUpdate(contact);

        Events.Add(new ContactRequestRejected(contact));
    }

    private void ReceiveContactUpdate(Contact contact)
    {
        // This if else could be worth refactoring into strategy pattern to avoid violating the
        // open-closed principle, but for now, it will likely suffice since there are finite forseeable states.
        if (contact.IsPending())
        {
            ContactsList.Add(contact);
        }
        else if (contact.HasRejected())
        {
            ContactsList.Remove(contact);
        }
    }
}
