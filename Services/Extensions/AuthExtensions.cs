using System.Text;
using api.Services.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace api.Services.Extensions
{
    public static class AuthExtensions
    {
        public static IServiceCollection AddAuthServices(this IServiceCollection services, string tokenSecret)
        {
            byte[] key = Encoding.ASCII.GetBytes(tokenSecret);

            services.AddScoped<TokenService>();

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; //TODO: reabilitar em producao
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("HasNoGroup", policy => policy.RequireClaim("Group", null));
            });

            services.AddTransient<UserService>();

            return services;
        }
    }
}
