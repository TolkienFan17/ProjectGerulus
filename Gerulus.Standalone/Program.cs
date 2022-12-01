using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Runtime.CompilerServices;
using Gerulus.Core;
using Gerulus.Core.Auth;
using Gerulus.Core.Crypto;
using Gerulus.Standalone.UserForms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Org.BouncyCastle.Asn1.Misc;
using Sharprompt;

namespace Gerulus.Standalone;

public class Application
{
    private IServiceProvider Services { get; }
    public Application()
    {
        var collection = new ServiceCollection();
        collection.AddDbContext<GerulusContext>();

        collection.AddSingleton<IEncryptionMessageService, EncryptionMessageService>();
        collection.AddSingleton<ICryptoKeyService<DHFGParameters>, DHFGKeyProvider>();
        collection.AddSingleton<IAuthenticationService, AuthenticationService>();

        var forms = new List<Type>()
        {
            typeof(MainMenuForm),
            typeof(CreateAccountUserForm), typeof(LoginUserForm),
            typeof(InboxUserForm)
        };

        foreach (var form in forms)
        {
            collection.AddSingleton(form, form);
        }

        Services = collection.BuildServiceProvider();
    }

    public Task RunAsync()
    {
        return Services.GetRequiredService<MainMenuForm>().ExecuteAsync();
    }
}

public class Program
{
    public static async Task Main()
    {
        var app = new Application();
        await app.RunAsync();
    }
}

