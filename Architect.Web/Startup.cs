using System.Text.Json;
using Architect.Migrator;
using Architect.Web.BLL;
using Architect.Web.DAL;
using Architect.Web.Mappings;
using Architect.Web.Middlewares;
using Architect.Web.Security;
using FluentValidation.AspNetCore;
using Mapster;

namespace Architect.Web;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        var authOptions = new AuthOptions
        {
            Issuer = _configuration.GetValue<string>("Application:Name"),
            SecretKey = _configuration.GetValue<string>("TOKEN_SECRET"),
            TokenLifetimeInDays = 2
        };

        var connectionString = _configuration.GetValue<string>("CONNECTION_STRING");

        services
            .AddHostedService<Warmup>()
            .Configure<AuthOptions>(
                options =>
                {
                    authOptions.Adapt(options);
                })
            .AddDatabase(connectionString)
            .AddMigrator(connectionString)
            .AddMapster()
            .AddBll()
            .AddAuth(authOptions)
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            })
            .AddFluentValidation(
                fv =>
                {
                    fv.RegisterValidatorsFromAssemblyContaining<Startup>();
                    fv.ImplicitlyValidateChildProperties = false;
                });

        services.AddScoped<ExceptionHandler>();
    }

    public void Configure(
        IApplicationBuilder app,
        IWebHostEnvironment env)
    {
        app.UseAuthentication();
        app.UseRouting();
        app.UseExceptionHandler(
            exceptionHandlerApp =>
            {
                exceptionHandlerApp.Run(
                    async context =>
                    {
                        using var scope = context.RequestServices.CreateScope();
                        var handler = scope.ServiceProvider.GetRequiredService<ExceptionHandler>();
                        await handler.Handle(context);
                    });
            });
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}

