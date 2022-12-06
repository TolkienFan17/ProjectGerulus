namespace Gerulus.Standalone;

public class Program
{
    public static async Task Main()
    {
        using var app = new Application();
        await app.RunAsync();
    }
}

