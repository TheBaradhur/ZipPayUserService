using Microsoft.Extensions.DependencyInjection;

namespace ZipPay.User.Domain
{
    public static class DomainExtensions
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAccountService, AccountService>();

            return services;
        }
    }
}