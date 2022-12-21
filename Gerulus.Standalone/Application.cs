using Autofac;
using Gerulus.Core.Auth;
using Gerulus.Core.Crypto;
using Gerulus.Core.Crypto.Messages;
using Gerulus.Core.Database;
using Gerulus.Standalone.UserForms;

namespace Gerulus.Standalone;

public class Application : IDisposable, IAsyncDisposable
{
    private IContainer? Container { get; set; }
    private ILifetimeScope? ServicesScope { get; set; }

    public Application()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<DHFG.KeyProvider>()
               .As<ICryptoKeyProvider>();

        builder.RegisterType<DHFG.FileParameterProvider>()
               .As<ICryptoParameterProvider<DHFG.Parameters>>()
               .SingleInstance();

        builder.RegisterType<EncryptionMessageService>()
               .As<IEncryptionMessageService>();

        builder.RegisterType<AuthenticationService>()
               .As<IAuthenticationService>();

        builder.RegisterType<LocalUserState>()
               .AsSelf()
               .SingleInstance();

        builder.RegisterAssemblyTypes(typeof(IUserForm).Assembly)
               .Where(type => typeof(IUserForm).IsAssignableFrom(type))
               .AsSelf();

        builder.RegisterType<DefaultDatabaseOptionsProvider>()
               .As<IDatabaseOptionsProvider>();

        builder.RegisterType<GerulusContext>()
               .AsSelf();

        Container = builder.Build();
        ServicesScope = Container.BeginLifetimeScope();
    }

    public async Task RunAsync()
    {
        if (ServicesScope is null)
            throw new InvalidOperationException("Application has already been disposed");


        using (var scope = ServicesScope.BeginLifetimeScope())
        {
            var context = scope.Resolve<GerulusContext>();
            await context.Database.EnsureCreatedAsync();
        }

        var form = ServicesScope.Resolve<MainMenuForm>();
        await form.ExecuteAsync();
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);

        Dispose(false);
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
        GC.SuppressFinalize(this);
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            ServicesScope?.Dispose();
            Container?.Dispose();

            ServicesScope = null;
            Container = null;
        }
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        if (ServicesScope is not null)
            await ServicesScope.DisposeAsync().ConfigureAwait(false);

        if (Container is not null)
            await Container.DisposeAsync().ConfigureAwait(false);

        ServicesScope = null;
        Container = null;
    }

}

