using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FactoryScheduler.Authentication.Service.Dtos;
using FactoryScheduler.Authentication.Service.Entities;
using FactoryScheduler.Authentication.Service.Interfaces;
using FactoryScheduler.Authentication.Service.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static IdentityServer4.IdentityServerConstants;

//TODO: Add user certification table, only allow user (their own) or admin or planner to view user certs
namespace FactoryScheduler.Authentication.Service.Controllers
{
    [ApiController]
    [Route("buildings")]
    public class BuildingsController : ControllerBase
    {
        private readonly IMongoBaseRepository<WorkBuilding> _workBuildingRepository;
        private readonly IMongoBaseRepository<WorkArea> _workAreaRepository;
        public BuildingsController(IMongoBaseRepository<WorkBuilding> workBuildingRepository, IMongoBaseRepository<WorkArea> workAreaRepository)
        {
            _workBuildingRepository = workBuildingRepository;
            _workAreaRepository = workAreaRepository;

        }

        [HttpGet]
        public async Task<IEnumerable<WorkBuildingDto>> GetBuildingsAsync()
        {
            var workBuildings = await _workBuildingRepository.GetAllAsync();
            return workBuildings.Select(p => p.AsDto());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WorkBuildingDto>> GetBuildingByIdAsync([FromRoute] Guid id)
        {
            var building = await _workBuildingRepository.GetOneAsync(id);
            if (building == null)
            {
                return NotFound();
            }

            return building.AsDto();
        }

        [HttpGet("workAreas/{id}")]
        public async Task<ActionResult<IEnumerable<WorkAreaDto>>> GetBuildingWorkAreas([FromRoute] Guid id)
        {
            var building = await _workBuildingRepository.GetOneAsync(id);
            if (building == null)
            {
                return NotFound();
            }
            var buildingWorkAreas = await _workAreaRepository.GetAllAsync(workArea => workArea.WorkBuildingId == building.Id);
            var workAreaDtos = buildingWorkAreas?.Select(workArea => workArea.AsDto(building.Name, building.Description));
            return Ok(workAreaDtos);
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin, Policy = LocalApi.PolicyName)]
        public async Task<ActionResult<WorkBuildingDto>> AddBuilding(CreateWorkBuildingDto createBuildingDto)
        {
            var workBuilding = new WorkBuilding
            {
                CreatedDate = DateTimeOffset.UtcNow,
                Description = createBuildingDto.Description,
                Id = Guid.NewGuid(),
                Name = createBuildingDto.Name
            };
            await _workBuildingRepository.CreateAsync(workBuilding);

            return CreatedAtAction(nameof(GetBuildingByIdAsync), new { id = workBuilding.Id }, workBuilding);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> UpdateBuildingAsync([FromRoute] Guid id, UpdateWorkBuildingDto updateBuildingDto)
        {
            var building = await _workBuildingRepository.GetOneAsync(id);
            if (building == null)
            {
                return NotFound();
            }
            building.Description = updateBuildingDto.Description;
            building.Name = updateBuildingDto.Name;


            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> DeleteBuildingAsync([FromRoute] Guid id)
        {
            var building = await _workBuildingRepository.GetOneAsync(id);
            if (building == null)
            {
                return NotFound();
            }
            await _workBuildingRepository.RemoveAsync(id);

            return NoContent();
        }
    }
}