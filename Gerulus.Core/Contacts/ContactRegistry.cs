using Gerulus.Core.Crypto;
using Gerulus.Core.Database;
using Microsoft.EntityFrameworkCore;

namespace Gerulus.Core.Contacts;

public class ContactRegistry : IContactRegistry
{
    public required ICryptoKeyProvider KeyProvider { get; init; }
    public required GerulusContext Context { get; init; }

    public async Task<AttackResult> AttackContactAsync(Contact contact, User attacker, ContactAttackType desiredAttack)
    {
        if (!contact.IsPending)
        {
            return AttackResult.FromContactCompletion(attacker, contact, desiredAttack);
        }

        contact.ActualRecipient = attacker;

        var victimKey = await KeyProvider.ComputeSharedKeyAsync(contact.ActualSender.GetKeyPair(), attacker.GetKeyPair());
        contact.SharedSecret = victimKey;
        contact.IsPending = false;

        switch (desiredAttack)
        {
            case ContactAttackType.Eavesdrop:

                var recipientUser = new Contact()
                {
                    ActualSender = attacker,
                    IntendedSender = contact.IntendedSender,
                    ActualRecipient = contact.IntendedRecipient,
                    IntendedRecipient = contact.IntendedRecipient,

                    IsPending = false
                };

                var recipientKey = await KeyProvider.ComputeSharedKeyAsync(attacker.GetKeyPair(), contact.IntendedRecipient.GetKeyPair());
                recipientUser.SharedSecret = recipientKey;

                await Context.Contacts.AddAsync(recipientUser);
                await Context.SaveChangesAsync();

                return AttackResult.FromEavesdropAttack(attacker, contact);

            case ContactAttackType.Intercept:
                contact.IsPending = false;
                await Context.SaveChangesAsync();
                return AttackResult.FromInterceptionAttack(attacker, contact);

            default:
                throw new InvalidOperationException($"Attack type {desiredAttack} is not supported");
        }
    }

    public async Task<Contact> CompleteContactAsync(User sender, User recipient)
    {
        var contact = await Context.Contacts.SingleOrDefaultAsync(contact => contact.ActualSender == sender && contact.IntendedRecipient == recipient);

        if (contact == null)
            throw new InvalidOperationException("A contact request has not been begun to be completed");

        contact.ActualRecipient = recipient;
        contact.IsPending = false;

        var key = await KeyProvider.ComputeSharedKeyAsync(sender.GetKeyPair(), recipient.GetKeyPair());
        contact.SharedSecret = key;

        var inverseContact = new Contact()
        {
            ActualSender = recipient,
            IntendedRecipient = sender,
            ActualRecipient = sender,
            IsPending = false,
            SharedSecret = key
        };

        await Context.AddAsync(inverseContact);
        await Context.SaveChangesAsync();
        return contact;
    }

    public async Task<Contact> CreateNewContactAsync(User sender, User recipient)
    {
        if (await DoesContactExistAsync(sender, recipient))
        {
            throw new InvalidOperationException("Cannot create a new contact between two users when a contact already exists");
        }

        var contact = new Contact()
        {
            ActualSender = sender,
            IntendedRecipient = recipient,
            IsPending = true
        };

        await Context.Contacts.AddAsync(contact);
        await Context.SaveChangesAsync();

        return contact;
    }

    public async Task<Contact> CreateOrCompleteContactAsync(User sender, User recipient)
    {
        if (await DoesContactExistAsync(sender, recipient))
        {
            return await CompleteContactAsync(sender, recipient);
        }

        var contact = new Contact()
        {
            ActualSender = sender,
            IntendedRecipient = recipient,
            IsPending = true
        };

        await Context.Contacts.AddAsync(contact);
        await Context.SaveChangesAsync();

        return contact;
    }

    // TODO Should the order matter?
    public Task<bool> DoesContactExistAsync(User sender, User recipient)
    {
        return Context.Contacts.AnyAsync(c => c.ActualSender == sender && c.IntendedRecipient == recipient);
    }
}
