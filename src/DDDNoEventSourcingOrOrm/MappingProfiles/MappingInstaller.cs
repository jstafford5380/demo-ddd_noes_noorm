using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace DDDNoEventSourcingOrOrm.MappingProfiles
{
    public static class MappingInstaller
    {
        public static void AddMappers(this IServiceCollection services)
        {
            services.AddAutoMapper(config => config.AddProfiles(
                Assembly.Load("Domain"), Assembly.Load("Interfaces")));
        }
    }
}
