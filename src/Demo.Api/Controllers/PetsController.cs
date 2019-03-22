using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Api.ViewModel;
using Demo.Application.Infrastructure;
using Demo.Application.UseCases;
using Demo.Application.UseCases.ManagingPets;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Controllers
{
    [Route("customers/{customerId}/pets")]
    public class PetsController : Controller
    {
        private readonly IManagePets _petManager;
        private readonly IMapper _mapper;

        public PetsController(IManagePets petManager, IMapper mapper)
        {
            _petManager = petManager;
            _mapper = mapper;
        }

        [HttpPost, Route("", Name = nameof(PostPetAsync))]
        public async Task<IActionResult> PostPetAsync(string customerId, [FromBody] PetInfo newPet)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tempState = _mapper.Map<PetState>(newPet);
            var result = await _petManager.AddPetAsync(customerId, tempState);

            if (result.IsSuccess)
                return Created($"/customers/{customerId}/pets/{result.Entity.PetId}", result.Entity);

            if (result.ResponseType == ResponseType.BusinessRuleViolation)
                return Conflict(result.Message);

            return StatusCode(500, "Unexpected result.");
        }

        [HttpPut, Route("{petId}")]
        public async Task<IActionResult> PutPetAsync(string customerId, string petId, [FromBody] PetInfo updatedPed)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedState = _mapper.Map<PetState>(updatedPed);
            updatedState.PetId = petId;

            var result = await _petManager.UpdatePetAsync(customerId, updatedState);

            if (result.IsSuccess)
                return Ok(result.Entity);

            if (result.ResponseType == ResponseType.EntityNotFound)
                return NotFound();

            if (result.ResponseType == ResponseType.BusinessRuleViolation)
                return Conflict(result.Message);

            return StatusCode(500, "Unexpected result.");
        }

        [HttpDelete, Route("{petId}")]
        public async Task<IActionResult> DeletePetAsync(string customerId, string petId)
        {
            var result = await _petManager.DeletePetAsync(customerId, petId);

            if (result.IsSuccess)
                return Ok();

            if (result.ResponseType == ResponseType.EntityNotFound)
                return NotFound();

            if (result.ResponseType == ResponseType.BusinessRuleViolation)
                return Conflict(result.Message);

            return StatusCode(500, "Unexpected result.");
        }
    }
}
