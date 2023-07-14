using ConsoleEF.Data;
using ConsoleEF.HostedServices;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Npgsql;

IHostBuilder hostBuilder = Host.CreateDefaultBuilder();

hostBuilder.ConfigureHostConfiguration(builder => { builder.AddUserSecrets<Program>(); });

hostBuilder.ConfigureServices((context, services) =>
{
    services.AddDbContextFactory<SchoolContext>(builder =>
    {
        IConfiguration configuration = context.Configuration;

        var connectionStringBuilder = new NpgsqlConnectionStringBuilder
        {
            Host = configuration["database:host"],
            Port = int.TryParse(configuration["database:port"], out int port) ? port : 26257,
            Username = configuration["database:username"],
            Password = configuration["database:password"],
            Database = "school",
        };

        string connectionString = connectionStringBuilder.ConnectionString;

        builder.UseNpgsql(connectionString);
    });

    services.AddTransient<StudentReadOnlyRepository>();
    services.AddTransient<StudentReadWriteRepository>();

    services.AddHostedService<ConsoleHostedService>();

    services.AddLogging();
});

hostBuilder.UseConsoleLifetime();

await hostBuilder.RunConsoleAsync().ConfigureAwait(false);