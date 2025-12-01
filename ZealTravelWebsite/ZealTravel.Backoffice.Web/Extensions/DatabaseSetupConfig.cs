using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ZealTravel.Infrastructure.Repository;
using ZealTravel.Domain.Interfaces.IRepository;
using ZealTravelWebsite.Infrastructure.Context;
using ZealTravel.Domain.Interfaces.AirelineManagement;
using ZealTravel.Infrastructure.Repository.AirlineManagement;
//using ZealTravel.Domain.Interfaces.User_Management;
namespace ZealTravel.Backoffice.Web.Extensions
{
    public static class DatabaseSetupConfig
    {
        public static IServiceCollection AddCustomDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ZealdbNContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            services.AddScoped<ICompanyRegisterRepository, Infrastructure.Repository.CompanyRegisterRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            //services.AddScoped<ICredentialRepository>();
            return services;
        }
    }
}
