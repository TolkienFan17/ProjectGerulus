namespace Gerulus.Core.Contacts;

public class Contact : IEntity
{
    public EntityId Id { get; init; } = new EntityId();

    public ContactUser Sender { get; init; }
    public ContactUser Recipient { get; private set; }
    
    private ContactState State { get; set; } = ContactState.PendingRequest;

    public Contact(ContactUser sender, ContactUser recipient)
    {
        Sender = sender;
        Recipient = recipient;
    }

    public bool IsPending() => State == ContactState.PendingRequest;
    public bool HasAccepted() => State == ContactState.AcceptedRequest;
    public bool HasRejected() => State == ContactState.RejectedRequest;

    public void Accept(ContactUser recipient)
    {
        // Should we verify this business rule in this entity, or let the fact that Contact is not an aggregate root 
        // and is only called by recipient ContactUser guarantee the integrity of this rule?
        //
        // if (!recipient.Identity.Equals(Recipient.Identity))
        //     throw new ArgumentException($"Recipient {recipient} cannot accept a request from {Sender}");

        State = ContactState.AcceptedRequest;
        Recipient = recipient;
    }

    public void Reject()
    {
        State = ContactState.RejectedRequest;
    }

    public bool IsIdenticalTo(ContactUser sender, ContactUser recipient)
    {
        return Sender.Identity.Equals(sender.Identity) && Recipient.Identity.Equals(recipient.Identity);
    }

}
