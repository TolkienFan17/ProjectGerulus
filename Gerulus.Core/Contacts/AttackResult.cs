namespace Gerulus.Core.Contacts;

public struct AttackResult
{
    public required User Attacker { get; init; }
    public required ContactAttackType AttackType { get; init; }
    public required Contact Contact { get; init; }

    public required bool IsSuccessful { get; init; }

    public static AttackResult FromEavesdrop(User attacker, Contact contact) => new()
    {
        Attacker = attacker,
        Contact = contact,
        AttackType = ContactAttackType.Eavesdrop,
        IsSuccessful = true
    };

    public static AttackResult FromIntercept(User attacker, Contact contact) => new()
    {
        Attacker = attacker,
        Contact = contact,
        AttackType = ContactAttackType.Intercept,
        IsSuccessful = true
    };

    public static AttackResult FromContactCompletion(User attacker, Contact contact, ContactAttackType intendedAttack) => new()
    {
        Attacker = attacker,
        Contact = contact,
        AttackType = intendedAttack,
        IsSuccessful = false
    };
}