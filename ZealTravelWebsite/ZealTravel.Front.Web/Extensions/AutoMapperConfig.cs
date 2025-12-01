using Microsoft.Extensions.DependencyInjection;
using ZealTravel.Application.Handlers;
using AutoMapper;
using ZealTravel.Application.Mappers;
using ZealTravel.Front.Web.Mappers;
namespace ZealTravel.Front.Web.Extensions
{
    public static class AutoMapperConfig
    {
        public static IServiceCollection AddCustomMappers(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
            services.AddAutoMapper(typeof(MapperProfile).Assembly);
            return services;
        }
    }
}
