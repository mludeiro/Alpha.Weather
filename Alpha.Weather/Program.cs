
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Alpha.Utils.Consul;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddHealthChecks();
        builder.Services.AddMemoryCache();

//      IdentityModelEventSource.ShowPII = true;

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ValidateIssuerSigningKey = false,
                    SignatureValidator = (string token, TokenValidationParameters parameters) => new JwtSecurityToken(token)
                };
            });
            
        builder.Services.AddAuthorizationBuilder()
            .AddPolicy("Weather.Weather.Read", authBuilder => { authBuilder.RequireClaim("Weather.Weather.Read"); });

        builder.Services.ConsulServicesConfig(builder.Configuration.GetSection("Consul").Get<ConsulConfig>()!);

        var app = builder.Build();
        app.MapControllers();
        app.MapHealthChecks("/health");

        
        app.UseAuthentication();
        app.UseAuthorization();



        app.Run();
    }
}