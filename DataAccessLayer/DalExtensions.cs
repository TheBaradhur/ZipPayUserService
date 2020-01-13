using Microsoft.Extensions.DependencyInjection;

namespace Dal
{
    public static class DalExtensions
    {
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, string connectionString)
        {
            services.AddTransient<IConnectionFactory>(s => new ConnectionFactory(connectionString));
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<IUserRepository, UserRepository>();

            return services;
        }
    }
}