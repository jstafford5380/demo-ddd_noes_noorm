﻿using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.MappingProfiles
{
    public static class MappingInstaller
    {
        public static void AddMappers(this IServiceCollection services)
        {
            services.AddAutoMapper(config => config.AddProfiles(
                Assembly.Load("Demo.Api"),
                Assembly.Load("Demo.Domain"),
                Assembly.Load("Demo.Application")));
        }
    }
}
