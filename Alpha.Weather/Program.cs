using Alpha.Common.Consul;
using Alpha.Common.Security;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddHealthChecks();
        builder.Services.AddMemoryCache();

//      IdentityModelEventSource.ShowPII = true;

        var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()!;
        builder.Services.AddAlphaAuthentication(jwtOptions);
            
        builder.Services.AddAuthorizationBuilder().AddAlphaAuthorizationPolicies();

//        builder.Services.ConsulServicesConfig(builder.Configuration.GetSection("Consul").Get<ConsulConfig>()!);

        var app = builder.Build();
        app.MapControllers();
        app.MapHealthChecks("/health");

        
        app.UseAuthentication();
        app.UseAuthorization();

        app.Run();
    }
}