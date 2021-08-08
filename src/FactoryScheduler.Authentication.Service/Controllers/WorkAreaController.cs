using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FactoryScheduler.Authentication.Service.Dtos;
using FactoryScheduler.Authentication.Service.Entities;
using FactoryScheduler.Authentication.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FactoryScheduler.Authentication.Service.Controllers
{
    [ApiController]
    [Route("workAreas")]
    public class WorkAreaController : ControllerBase
    {
        private readonly IMongoBaseRepository<WorkArea> _workAreaRepository;
        private readonly IMongoBaseRepository<WorkBuilding> _workBuildingRepository;
        private readonly IMongoBaseRepository<WorkStation> _workStationRepository;
        private readonly UserManager<FactorySchedulerUser> _userManager;

        public WorkAreaController(IMongoBaseRepository<WorkArea> workAreaRepository,
                                  IMongoBaseRepository<WorkBuilding> workBuildingRepository,
                                  IMongoBaseRepository<WorkStation> workStationRepository,
                                  UserManager<FactorySchedulerUser> userManager)
        {
            _workAreaRepository = workAreaRepository;
            _workBuildingRepository = workBuildingRepository;
            _workStationRepository = workStationRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkAreaDto>>> GetWorkAreasAsync()
        {
            var workAreas = await _workAreaRepository.GetAllAsync();
            var workAreaBuildingIds = workAreas.Select(p => p.WorkBuildingId);
            var workBuildings = await _workBuildingRepository.GetAllAsync(building => workAreaBuildingIds.Contains(building.Id));
            var workAreaDtos = workAreas.Select(workArea =>
            {
                var workBuilding = workBuildings.Single(building => building.Id == workArea.WorkBuildingId);
                return workArea.AsDto(workBuilding.Name, workBuilding.Description);
            });
            return Ok(workAreaDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WorkAreaDto>> GetWorkAreaByIdAsync([FromRoute] Guid id)
        {
            var item = await _workAreaRepository.GetOneAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            var workBuilding = (await _workBuildingRepository.GetFirstOrDefaultAsync(p => p.Id == item.WorkBuildingId))?.AsDto();
            if (workBuilding == null)
                return NotFound();

            return item.AsDto(workBuilding.Name, workBuilding.Description);
        }

        [HttpPost]
        public async Task<ActionResult<WorkAreaDto>> AddWorkAreaAsync([FromBody] CreateWorkAreaDto updateItemDto)
        {
            var item = new WorkArea
            {
                CreatedDate = DateTimeOffset.UtcNow,
                Description = updateItemDto.Description,
                Id = Guid.NewGuid(),
                Name = updateItemDto.Name,
                WorkBuildingId = updateItemDto.WorkBuildingId
            };
            await _workAreaRepository.CreateAsync(item);

            return CreatedAtAction(nameof(GetWorkAreaByIdAsync), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkAreaAsync([FromRoute] Guid id, [FromBody] UpdateWorkAreaDto updateItemDto)
        {
            var item = await _workAreaRepository.GetOneAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            item.Description = updateItemDto.Description;
            item.Name = updateItemDto.Name;

            await _workAreaRepository.UpdateAsync(item);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkAreaAsync([FromRoute] Guid id)
        {
            var item = await _workAreaRepository.GetOneAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            await _workAreaRepository.RemoveAsync(id);

            return NoContent();
        }

        [HttpGet("stations/{id}")]
        public async Task<ActionResult<IEnumerable<WorkStationDto>>> GetWorkAreaWorkStationsByIdAsync([FromRoute] Guid id)
        {
            var workArea = await _workAreaRepository.GetOneAsync(id);
            if (workArea == null)
            {
                return NotFound();
            }

            var workStations = await _workStationRepository.GetAllAsync(workStation => workStation.WorkAreaId == workArea.Id);
            var stationDtos = workStations?.Select(workStation =>
            {
                var workStationUsers = _userManager.Users.Where(p => p.AssignedWorkStationId == workStation.Id).AsWorkStationUsers();
                return workStation.AsDto(workArea.Name, workArea.Description, workStationUsers);
            });

            return Ok(stationDtos);
        }
    }
}