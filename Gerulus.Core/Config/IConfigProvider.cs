namespace Gerulus.Core.Config;

public interface IConfigProvider
{
    Task LoadAsync();
    AppConfiguration Get();
    Task SaveAsync();
}
