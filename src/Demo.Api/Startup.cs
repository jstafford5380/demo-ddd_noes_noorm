using AutoMapper;
using Demo.Api.DependencyInjection.MappingProfiles;
using Demo.Application.Infrastructure;
using Demo.Application.UseCases.ManagingPets;
using Demo.Domain.ManagePetContext.Services;
using Demo.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetManagementDomain = Demo.Domain.ManagePetContext.Services.PetManagementService;
using PetManagementApplication = Demo.Application.UseCases.ManagingPets.PetManagementService;

namespace Demo.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<StaticStorage<string, PersonState>>();
            services.AddScoped<IPersonRepository>(p =>
            {
                var repo = new PersonRepository(
                    p.GetRequiredService<StaticStorage<string, PersonState>>(), 
                    p.GetRequiredService<IMapper>());
                repo.Setup().Wait();
                return repo;
            });
            services.AddScoped<IPersonEventStream, PersonEventRepository>();
            services.AddScoped<IPetManagementService, PetManagementDomain>();
            services.AddScoped<IManagePets, PetManagementApplication>();
            services.AddMappers();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }

}
