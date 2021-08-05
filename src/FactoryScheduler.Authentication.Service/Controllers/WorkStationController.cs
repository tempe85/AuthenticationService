using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FactoryScheduler.Authentication.Service.Dtos;
using FactoryScheduler.Authentication.Service.Entities;
using FactoryScheduler.Authentication.Service.Interfaces;
using FactoryScheduler.Authentication.Service.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FactoryScheduler.Authentication.Service.Controllers
{
    [ApiController]
    [Route("workStations")]
    public class WorkStationController : ControllerBase
    {
        private readonly IMongoBaseRepository<WorkArea> _workAreaRepository;
        private readonly IMongoBaseRepository<WorkStation> _workStationRepository;
        private readonly UserManager<FactorySchedulerUser> _userManager;

        public WorkStationController(IMongoBaseRepository<WorkArea> workAreaRepository,
                                    IMongoBaseRepository<WorkStation> workStationRepository,
                                    UserManager<FactorySchedulerUser> userManager)
        {
            _workAreaRepository = workAreaRepository;
            _workStationRepository = workStationRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkStationsByWorkAreaModel>>> GetWorkStationsByWorkAreasAsync([FromBody] Guid[] workAreaIds)
        {
            var workStations = await _workStationRepository.GetAllAsync();
            var workAreas = await _workAreaRepository.GetAllAsync(p => workAreaIds.Contains(p.Id));

            var workStationsWorkAreaModel = workAreas.Select(workArea =>
            {
                var workAreaWorkStationDtos = workStations.Where(p => p.WorkAreaId == workArea.Id)?.Select(p => p.AsDto(workArea.Name, workArea.Description)).ToArray();
                return new WorkStationsByWorkAreaModel
                {
                    WorkAreaId = workArea.Id,
                    WorkStationDtos = workAreaWorkStationDtos
                };
            });
            return Ok(workStationsWorkAreaModel);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WorkStationDto>> GetWorkStationByIdAsync(Guid id)
        {
            var workStation = await _workStationRepository.GetOneAsync(id);
            if (workStation == null)
            {
                return NotFound();
            }
            var workArea = await _workAreaRepository.GetFirstOrDefaultAsync(p => p.Id == workStation.WorkAreaId);
            if (workArea == null)
                return NotFound();

            return workStation.AsDto(workArea.Name, workArea.Description);
        }

        [HttpPost]
        public async Task<ActionResult<WorkAreaDto>> AddWorkStationAsync(CreateWorkStationDto createWorkStationDto)
        {
            //First make sure workArea exists
            var workArea = await _workAreaRepository.GetOneAsync(createWorkStationDto.WorkAreaId);
            var maxOperationPosition = (await workArea.GetWorkAreaWorkStationsAsync(_workStationRepository))?.Select(p => p.WorkAreaPosition).ToArray().Max() ?? -1;
            if (workArea == null)
            {
                throw new Exception($"Unable to find work area: {createWorkStationDto.WorkAreaId} for created work station");
            }
            var workStation = new WorkStation
            {
                CreatedDate = DateTimeOffset.UtcNow,
                Description = createWorkStationDto.Description,
                Id = Guid.NewGuid(),
                Name = createWorkStationDto.Name,
                WorkAreaPosition = maxOperationPosition + 1,
                WorkerCapacity = createWorkStationDto.WorkerCapacity,
                WorkStationType = createWorkStationDto.WorkStationType,
            };
            await _workStationRepository.CreateAsync(workStation);

            return CreatedAtAction(nameof(GetWorkStationByIdAsync), new { id = workStation.Id }, workStation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkStationAsync(Guid id, UpdateWorkStationDto updateWorkStationDto)
        {
            var workStation = await _workStationRepository.GetOneAsync(id);
            if (workStation == null)
            {
                return NotFound();
            }
            workStation.Description = updateWorkStationDto.Description;
            workStation.Name = updateWorkStationDto.Name;
            workStation.isDeleted = updateWorkStationDto.isDeleted;

            await _workStationRepository.UpdateAsync(workStation);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkStationAsync(Guid id)
        {
            var item = await _workStationRepository.GetOneAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            await _workStationRepository.RemoveAsync(id);

            return NoContent();
        }

        [HttpGet("stations/{id}")]
        public async Task<ActionResult<IEnumerable<WorkStation>>> GetWorkersByWorkStationAsync([FromRoute] Guid id)
        {
            var workStation = await _workStationRepository.GetOneAsync(id);
            if (workArea == null)
            {
                return NotFound();
            }

            var workStations = await _workStationRepository.GetAllAsync(workStation => workStation.WorkAreaId == workArea.Id);
            var stationDtos = workStations?.Select(workStation => workStation.AsDto(workArea.Name, workArea.Description));

            return Ok(stationDtos);
        }
    }
}