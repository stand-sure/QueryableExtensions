namespace Service;

public static class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Host.ConfigureHostBuilder();

        builder.Services.ConfigureServices(builder.Configuration);

        WebApplication app = builder.Build();

        app.ConfigureApplicationBuilder();

        app.ConfigureEndpointRouteBuilder();

        app.Run();
    }
}