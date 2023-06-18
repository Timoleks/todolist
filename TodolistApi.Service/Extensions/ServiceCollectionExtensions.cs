using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TodolistApi.Domain.Models;
using TodolistApi.Service.Repository;

namespace TodolistApi.Service.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTodolistCrudService(this IServiceCollection services) =>
            services.AddScoped<IRepository<TodoItem>, Repository<TodoItem>>();

        public static IServiceCollection AddJwtAuthorization(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var secret = "SomeSecresadfsdafasdfasdfasdfasddfasdgasdfgadgsdafsadfasdfasdt";
                    var issuer = "KanyaFieldsIdentityProvider";
                    var audience = "KanyaFieldsUsers";
                    
                    var publicKeyBytes = Encoding.UTF8.GetBytes(secret);
                    // TODO: Use asymmetric encryption or certificate
                    var key = new SymmetricSecurityKey(publicKeyBytes);

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateLifetime = true,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        ValidateAudience = true,
                        ValidAudience = audience,
                        ValidIssuer = issuer,
                        IssuerSigningKey = key
                    };
                });

            return services;
        }
    }
}
