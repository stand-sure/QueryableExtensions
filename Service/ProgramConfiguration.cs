namespace Service;

using System.Reflection;

using ConsoleEF.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using Npgsql;

using Prometheus;

using Serilog;

using Service.Data;
using Service.Filters;

using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

internal static class ProgramConfiguration
{
    private const string OpenApiDocumentName = "openapi";

    public static void ConfigureApplicationBuilder(this IApplicationBuilder app)
    {
        app.UseSwagger(ConfigureSwaggerOptions);

        app.UseSwaggerUI(ConfigureSwaggerUiOptions);

        app.UseSerilogRequestLogging();

        app.UseHttpMetrics();

        app.UseHttpsRedirection();

        app.UseAuthorization();
    }

    public static void ConfigureEndpointRouteBuilder(this IEndpointRouteBuilder app)
    {
        app.MapMetrics("/metricsz");

        app.MapHealthChecks("/healthz");

        app.MapControllers();
    }

    public static void ConfigureHostBuilder(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog(ConfigureHostSerilog);
    }

    private static void ConfigureHostSerilog(HostBuilderContext context, IServiceProvider provider, LoggerConfiguration loggerConfig)
    {
        loggerConfig
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(provider);
    }

    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLogging();

        services.AddHealthChecks();

        services.AddControllers(ConfigureMvcOptions);

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(ConfigureSwaggerGenOptions);

        services.AddDbContextFactory<SchoolContext>(builder => ConfigureDbContextFactory(builder, configuration));

        services.AddScoped<StudentReadOnlyRepository>();
        services.AddScoped<StudentReadWriteRepository>();
        
        return services;
    }

    private static void ConfigureDbContextFactory(this DbContextOptionsBuilder builder, IConfiguration configuration)
    {
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
    }

    private static void ConfigureMvcOptions(MvcOptions options)
    {
        options.Conventions.Add(new RouteTokenTransformerConvention(new QueryMutationControllerParameterTransformer()));

        options.Filters.Add(new ModelValidationActionFilterAttribute());
    }

    private static void ConfigureSwaggerGenOptions(SwaggerGenOptions options)
    {
        var openApiInfo = new OpenApiInfo
        {
            Title = "Demo API",
            Version = "v1",
            Description = "Demo", 
        };

        options.SwaggerDoc(ProgramConfiguration.OpenApiDocumentName, openApiInfo);

        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        
        options.CustomOperationIds(GenerateOperationId);
    }

    private static string? GenerateOperationId(ApiDescription description)
    {
        // {controllerName}_{operationName}
        description.TryGetMethodInfo(out MethodInfo methodInfo);
        string? controllerName = (description.ActionDescriptor as ControllerActionDescriptor)?.ControllerName;
        string? operationName = description.ActionDescriptor.AttributeRouteInfo?.Name ?? methodInfo?.Name;

        string? operationId = (controllerName, operationName) switch
        {
            (_, null) => null,
            _ => $"{controllerName}_{operationName}",
        };

        return operationId;
    }

    private static void ConfigureSwaggerOptions(SwaggerOptions options)
    {
        options.SerializeAsV2 = false;
        options.RouteTemplate = "swagger/{documentName}/swagger.json";
    }

    private static void ConfigureSwaggerUiOptions(SwaggerUIOptions options)
    {
        options.ConfigObject ??= new ConfigObject();
        options.ConfigObject.ShowCommonExtensions = true;
        options.ConfigObject.ShowExtensions = true;
        options.DisplayOperationId();

        options.SwaggerEndpoint($"/swagger/{ProgramConfiguration.OpenApiDocumentName}/swagger.json", "OpenAPI v3");
    }
}