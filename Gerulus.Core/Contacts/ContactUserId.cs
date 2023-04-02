namespace Gerulus.Core.Contacts;

public readonly record struct ContactUserId(UserId Core, UserId Identity) : IEquatable<ContactUserId>;
