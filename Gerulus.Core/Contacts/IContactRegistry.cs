namespace Gerulus.Core.Contacts;

public interface IContactRegistry
{
    Task<Contact> CreateNewContactAsync(User primary, User secondary);
    Task<Contact> CompleteContactAsync(User primary, User secondary);
    Task<Contact> CreateOrCompleteContactAsync(User primary, User secondary);

    Task<AttackResult> AttackContactAsync(Contact contact, User attacker, ContactAttackType desiredAttack);

    Task<bool> DoesContactExistAsync(User primary, User secondary);
}
