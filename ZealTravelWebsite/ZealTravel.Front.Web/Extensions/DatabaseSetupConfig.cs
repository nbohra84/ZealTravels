using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ZealTravel.Infrastructure.Repository;
using ZealTravel.Domain.Interfaces.IRepository;
using ZealTravelWebsite.Infrastructure.Context;
using ZealTravel.Domain.Interfaces.FlightManagement;
using ZealTravel.Infrastructure.FlightManagement;
using ZealTravel.Domain.Interfaces.TBO;
using ZealTravel.Infrastructure.TBO;
using ZealTravel.Domain.Interfaces.AirCalculation;
using ZealTravel.Infrastructure.AirCalculations;
using ZealTravel.Domain.Interfaces.DBCommon;
using ZealTravel.Infrastructure.DBCommon;
using ZealTravel.Infrastructure.UAPI;
using GetTBOServices = ZealTravel.Infrastructure.TBO.GetServices;
using ZealTravel.Infrastructure.Akaasha;
using ZealTravel.Domain.Interfaces.Akasha;
using ZealTravel.Domain.Interfaces.AirelineManagement;
using ZealTravel.Infrastructure.Repository.AirlineManagement;
using ZealTravel.Domain.Interfaces.FlightManagement.Akasaa;
using ZealTravel.Domain.Interfaces.AirlineManagement;
using ZealTravel.Domain.Services.AirelineManagement;
using ZealTravel.Domain.Interfaces.AirlineManagement.Akasaa;
using ZealTravel.Domain.Interfaces.Whitelabel;
using ZealTravel.Infrastructure.Repository.Whitelabel;
using ZealTravel.Domain.Interfaces.PromoManagement;
using ZealTravel.Infrastructure.Repository.PromorManagement;
using ZealTravel.Domain.Interfaces.CountryManagement;
using ZealTravel.Infrastructure.Services.Country;
using ZealTravel.Domain.Interfaces.PaymentGatewayManagement;
using ZealTravel.Infrastructure.Services.Aireline;
using ZealTravel.Domain.Interfaces.GSTManagement;
using ZealTravel.Domain.Interfaces.PNRManagement;
using ZealTravel.Domain.Interfaces.CallCenterManagement;
using ZealTravel.Domain.BookingManagement;
using ZealTravel.Domain.Services.AirelineManagement.Booking;
using ZealTravel.Infrastructure.Services.DBLogger;


namespace ZealTravel.Front.Web.Extensions
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
            services.AddScoped<IBookingManagementService, ZealTravel.Infrastructure.Services.BookingService>();
            services.AddScoped<Domain.Interfaces.TBO.IGetServices, GetTBOServices>();
            services.AddScoped<Domain.Interfaces.FlightManagement.Akasaa.IGetServices, Infrastructure.Akaasha.GetServices>();
            services.AddScoped<Domain.Interfaces.UniversalAPI.IGetServices, ZealTravel.Infrastructure.UAPI.GetServices>();
            services.AddScoped<Domain.Interfaces.FlightManagement.Spicejet.IGetServices, ZealTravel.Infrastructure.Spicejet.GetServices>();
            services.AddScoped<IGetFare, ZealTravel.Infrastructure.Akaasha.GetFare>();
            services.AddScoped<IGetAirFareService, GetAirFareService>();
            services.AddScoped<IGetAirFareRulesService, GetAirFareRulesService>();
            services.AddScoped<IRR_Layer, rr_Layer>();
            services.AddScoped<ICredential, Credential>();
            services.AddScoped<ITI_DBData, TI_dbData>();
            services.AddScoped<ITI_Search, TI_Search>();
            services.AddScoped<IGetResponseData, GetResponseData>();
            services.AddScoped<IGetApiFlightFareRule, ZealTravel.Infrastructure.Akaasha.GetApiFlightFareRule>();
            services.AddScoped<IGetApiAvailabilityFare, ZealTravel.Infrastructure.Akaasha.GetApiAvailabilityFare>();
            services.AddScoped<IAirlineDetailService, AirlineDetail>();
            services.AddScoped<ICompanyRegisterCorporateUsersRepository, CompanyRegisterCorporateUserRepository>();
            services.AddScoped<ICompanyRegisterCorporateUsersLimitRepository, CompanyRegisterCorporateUsersLimitRepository>();
            services.AddScoped<IWhitelabelDetailRepository, WhitelabelDetailRepository>();
            services.AddScoped<ICompanyPromoDetailRepository, CompanyPromoDetailRepository>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IGetAirSSRService, GetAirSSRService>();
            services.AddScoped<IPaymentGatewayService, PaymentGatewayService>();
            services.AddScoped<IGSTService, GSTService>();
            services.AddScoped<IPNRService, PNRService>();
            services.AddScoped<ICallCenterService, CallCenterService>();
            services.AddScoped<IAirCommitService, AirCommitService>();
            services.AddScoped<IBookingService, Domain.Services.AirelineManagement.Booking.BookingService>();
            services.AddScoped<IDBLoggerService, DBLoggerService>();
            return services;
        }
    }
}
