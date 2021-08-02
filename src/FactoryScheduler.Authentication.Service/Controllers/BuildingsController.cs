using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FactoryScheduler.Authentication.Service.Dtos;
using FactoryScheduler.Authentication.Service.Entities;
using FactoryScheduler.Authentication.Service.Interfaces;
using FactoryScheduler.Authentication.Service.Repositories;
using Microsoft.AspNetCore.Mvc;

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

        private static readonly List<BuildingDto> buildings = new()
        {
            new BuildingDto(Guid.NewGuid(), "Building 1", "Place where we build things", DateTimeOffset.UtcNow),
            new BuildingDto(Guid.NewGuid(), "Building 2", "Place where we build things2", DateTimeOffset.UtcNow)
        };

        [HttpGet]
        public async Task<IEnumerable<BuildingDto>> GetBuildingsAsync()
        {
            var workBuildings = await _workBuildingRepository.GetAllAsync();
            return workBuildings.Select(p => p.AsDto());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BuildingDto>> GetBuildingByIdAsync([FromRoute] Guid id)
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
            var buildingDtos = buildingWorkAreas?.Select(workArea => workArea.AsDto(building.Name, building.Description));
            return Ok(buildingDtos);
        }

        [HttpPost]
        public async Task<ActionResult<BuildingDto>> AddBuilding(CreateBuildingDto createBuildingDto)
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
        public async Task<IActionResult> UpdateBuildingAsync([FromRoute] Guid id, UpdateBuildingDto updateBuildingDto)
        {
            var building = await _workBuildingRepository.GetOneAsync(id);
            if (building == null)
            {
                return NotFound();
            }
            building.Description = updateBuildingDto.Description;
            building.Name = updateBuildingDto.Name;


            return NoContent();
        }

        [HttpDelete("{id}")]
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