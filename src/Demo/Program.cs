using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Application;
using Demo.Application.UseCases;
using Demo.Application.UseCases.ManagingPets;
using Demo.Domain.ManagePetContext.Model;
using Demo.Domain.ManagePetContext.Services;
using Demo.Domain.Shared;
using Demo.MappingProfiles;
using Demo.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PersonRepository = Demo.Infrastructure.Data.PersonRepository;
using PetManagementApplication = Demo.Domain.ManagePetContext.Services.PetManagementService;
using PetManagementDomain = Demo.Application.UseCases.ManagingPets.PetManagementService;

namespace Demo
{
    class Program
    {
        private IServiceProvider _services;

        private async Task RunDemo()
        {
            // example injected services at controller
            var mapper = _services.GetRequiredService<IMapper>();
            var mgtService = _services.GetRequiredService<IManagePets>();

            // example payloads coming in from API
            const string customerId = "person.foo";
            var pet1 = new NewPetInfo {Name = "fido", SpeciesId = 1, IsActive = true};
            var pet2 = new NewPetInfo {Name = "maxx", SpeciesId = 1, IsActive = true};

            /* BEGIN */

            var pet1State = mapper.Map<PetState>(pet1);

            var addPet1Result = await mgtService.AddPetAsync(customerId, pet1State);
            OutputResult("ADD_PET_SUNNY_DAY", addPet1Result);

            // demonstrate the domain protecting itself by enforcing uniqueness rule
            var addDuplicateResult = await mgtService.AddPetAsync(customerId, pet1State);
            OutputResult("ADD_PET_DUPLICATE", addDuplicateResult);

            // delete the duplicate and try again
            var deletePet1Result = await mgtService.DeletePetAsync(customerId, addPet1Result.Entity.PetId);
            OutputResult("DELETE_PET_BEFORE_ADD", deletePet1Result);

            var readdPet1Result = await mgtService.AddPetAsync(customerId, pet1State); // now succeeds
            OutputResult("READD_PET1", readdPet1Result);
            
            // add another pet
            var pet2State = mapper.Map<PetState>(pet2);
            var addPet2Result = await mgtService.AddPetAsync(customerId, pet2State);
            OutputResult("ADD_PET_2", addPet2Result);

            // try to change a pet's name to something that is already loaded against the customer
            var invalidUpdatePet = readdPet1Result.Entity;
            invalidUpdatePet.Name = "maxx";
            var invalidChangeResult = await mgtService.UpdatePetAsync(customerId, invalidUpdatePet);
            OutputResult("INVALID_UPDATE", invalidChangeResult);

            // demonstrate that "uniqueness" business rule is applied in a different way
            var cat = Pet.Create("fido", new SpeciesId(2));
            var catState = mapper.Map<PetState>(cat);
            var addCatResult = await mgtService.AddPetAsync(customerId, catState); // same name, but is ok because different species
            OutputResult("ADD_DIFF_SPECI_SAME_NAME", addCatResult);

            // retrieve the person and test
            var personRepo = _services.GetRequiredService<IPersonRepository>();
            var thePerson = await personRepo.GetAsync(customerId);
            var petNames =
                $"{string.Join(", ", thePerson.Pets.Take(thePerson.Pets.Count - 1).Select(p => p.Name))}, and {thePerson.Pets.Last().Name}";

            Console.WriteLine($"The person has {thePerson.Pets.Count} pets: {petNames}!");
            Console.ReadKey();
        }

        private static void OutputResult<T>(string description, ApplicationResponse<T> result)
        {
            var obj = new
            {
                DEMO_STEP = description,
                Response = result
            };
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            Trace.TraceInformation(json);
        }

        private async Task Setup()
        {
            var repo = _services.GetRequiredService<IPersonRepository>();
            var existingPerson = repo.GetAsync("person.foo");
            if (existingPerson == null)
            {
                var dp = new PersonState { PersonId = "person.foo", Pets = new List<PetState>() };
                await repo.SavePersonAsync(dp);
            }
        }

        private static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddSingleton<IPersonRepository, PersonRepository>();
            services.AddTransient<IPetManagementService, PetManagementApplication>();
            services.AddTransient<IManagePets, PetManagementDomain>();
            services.AddMappers();

            var program = new Program();
            program._services = services.BuildServiceProvider();
            await program.Setup();
            await program.RunDemo();
            Console.ReadKey();
        }
    }
}
