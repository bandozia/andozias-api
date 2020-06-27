using api.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace api.Services.Extensions
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<UserRepository>();
            services.AddTransient<GroupRepository>();
            services.AddTransient<RoleRepository>();
            return services;
        }
    }
}
