using Gerulus.Core.Crypto;
using Microsoft.EntityFrameworkCore;

namespace Gerulus.Core.Contacts;

public class ContactRegistry : IContactRegistry
{
    public required ICryptoKeyProvider KeyProvider { get; init; }

    public Task<AttackResult> AttackContactAsync(Contact contact, User attacker, ContactAttackType desiredAttack)
    {
        throw new NotImplementedException();
    }

    public async Task<Contact> CompleteContactAsync(User primary, User secondary)
    {
        using var context = new GerulusContext();
        var contact = await context.Contacts.SingleOrDefaultAsync(contact => contact.PrincipalUser == primary && contact.IntendedUser == secondary);

        if (contact == null)
            throw new InvalidOperationException("A contact request has not been begun to be completed");

        contact.State = ContactState.Completed;

        var key = await KeyProvider.ComputeSharedKeyAsync(primary.CreateKeyPair(), secondary.CreateKeyPair());
        contact.SharedSecret = key;

        await context.SaveChangesAsync();
        return contact;
    }

    public async Task<Contact> CreateNewContactAsync(User primary, User secondary)
    {
        if (await DoesContactExistAsync(primary, secondary))
        {
            throw new InvalidOperationException("Cannot create a new contact between two users when a contact already exists");
        }

        using var context = new GerulusContext();
        var contact = new Contact()
        {
            PrincipalUser = primary,
            IntendedUser = secondary,
            State = ContactState.Initiated
        };

        await context.Contacts.AddAsync(contact);
        await context.SaveChangesAsync();

        return contact;
    }

    public async Task<Contact> CreateOrCompleteContactAsync(User primary, User secondary)
    {
        if (await DoesContactExistAsync(primary, secondary))
        {
            return await CompleteContactAsync(primary, secondary);
        }

        using var context = new GerulusContext();
        var contact = new Contact()
        {
            PrincipalUser = primary,
            IntendedUser = secondary,
            State = ContactState.Initiated
        };

        await context.Contacts.AddAsync(contact);
        await context.SaveChangesAsync();

        return contact;
    }

    public Task<bool> DoesContactExistAsync(User primary, User secondary)
    {
        using var context = new GerulusContext();
        return context.Contacts.AnyAsync(c => c.PrincipalUser == primary && c.IntendedUser == secondary);
    }
}
