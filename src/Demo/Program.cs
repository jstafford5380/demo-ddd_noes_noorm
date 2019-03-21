using System;
using System.Linq;
using System.Threading.Tasks;
using Demo.Domain.Model;
using Demo.Domain.Model.ManagePet;
using Demo.Domain.Services;
using Demo.Domain.Shared;
using Demo.Interface;
using Demo.MappingProfiles;
using Microsoft.Extensions.DependencyInjection;
using PersonRepository = Demo.Infrastructure.Data.PersonRepository;

namespace Demo
{
    class Program
    {
        private IServiceProvider _services;

        private static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddSingleton<IPersonStore, PersonStoreAdapter>();
            services.AddSingleton<IPersonRepository, PersonRepository>();
            services.AddTransient<IPetManagementService, PetManagementService>();
            services.AddMappers();

            var program = new Program();
            program._services = services.BuildServiceProvider();

            program.RunDemo().GetAwaiter().GetResult();
            Console.ReadKey();
        }

        private async Task RunDemo()
        {
            // a service isn't really needed here, but it's here just to illustrate the layer isolation
            var mgtService = _services.GetRequiredService<IPetManagementService>();
            var newPerson = Person.Create(); // let's pretend this came from a user store

            var pet1 = Pet.Create("fido", new SpeciesId(1));
            var pet2 = Pet.Create("max", new SpeciesId(1));

            await mgtService.AddPetAsync(newPerson, pet1);

            try
            {
                // demonstrate the domain protecting itself by enforcing uniqueness rule
                await mgtService.AddPetAsync(newPerson, pet1);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }

            // delete the duplicate and try again
            await mgtService.DeletePetAsync(newPerson, pet1.PetId);
            await mgtService.AddPetAsync(newPerson, pet1);
            await mgtService.AddPetAsync(newPerson, pet2);

            // demonstrate that "uniqueness" business rule is applied in a different way
            var cat = Pet.Create("fido", new SpeciesId(2));
            await mgtService.AddPetAsync(newPerson, cat); // is ok because different species

            var petNames =
                $"{string.Join(", ", newPerson.Pets.Take(newPerson.Pets.Count - 1).Select(p => p.Name))}, and {newPerson.Pets.Last().Name}";


            Console.WriteLine($"The person has {newPerson.Pets.Count} pets: {petNames}!");
            Console.ReadKey();
        }
    }
}
