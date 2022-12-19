using Gerulus.Core.Crypto;
using Gerulus.Core.Database;
using Microsoft.EntityFrameworkCore;

namespace Gerulus.Core.Contacts;

public class ContactRegistry : IContactRegistry
{
    public required ICryptoKeyProvider KeyProvider { get; init; }
    public required GerulusContext Context { get; init; }

    public Task<AttackResult> AttackContactAsync(Contact contact, User attacker, ContactAttackType desiredAttack)
    {
        throw new NotImplementedException();
    }

    public async Task<Contact> CompleteContactAsync(User primary, User secondary)
    {
        var contact = await Context.Contacts.SingleOrDefaultAsync(contact => contact.ActualSender == primary && contact.IntendedRecipient == secondary);

        if (contact == null)
            throw new InvalidOperationException("A contact request has not been begun to be completed");

        contact.ActualRecipient = secondary;
        contact.IsPending = false;

        var key = await KeyProvider.ComputeSharedKeyAsync(primary.GetKeyPair(), secondary.GetKeyPair());
        contact.SharedSecret = key;

        var inverseContact = new Contact()
        {
            ActualSender = secondary,
            IntendedRecipient = primary,
            ActualRecipient = primary,
            IsPending = false,
            SharedSecret = key
        };

        await Context.AddAsync(inverseContact);
        await Context.SaveChangesAsync();
        return contact;
    }

    public async Task<Contact> CreateNewContactAsync(User primary, User secondary)
    {
        if (await DoesContactExistAsync(primary, secondary))
        {
            throw new InvalidOperationException("Cannot create a new contact between two users when a contact already exists");
        }

        var contact = new Contact()
        {
            ActualSender = primary,
            IntendedRecipient = secondary,
            IsPending = true
        };

        await Context.Contacts.AddAsync(contact);
        await Context.SaveChangesAsync();

        return contact;
    }

    public async Task<Contact> CreateOrCompleteContactAsync(User primary, User secondary)
    {
        if (await DoesContactExistAsync(primary, secondary))
        {
            return await CompleteContactAsync(primary, secondary);
        }

        var contact = new Contact()
        {
            ActualSender = primary,
            IntendedRecipient = secondary,
            IsPending = true
        };

        await Context.Contacts.AddAsync(contact);
        await Context.SaveChangesAsync();

        return contact;
    }

    public Task<bool> DoesContactExistAsync(User primary, User secondary)
    {
        return Context.Contacts.AnyAsync(c => c.ActualSender == primary && c.IntendedRecipient == secondary);
    }
}
