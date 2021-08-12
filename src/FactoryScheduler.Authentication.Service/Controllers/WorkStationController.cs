using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FactoryScheduler.Authentication.Service.Dtos;
using FactoryScheduler.Authentication.Service.Entities;
using FactoryScheduler.Authentication.Service.Interfaces;
using FactoryScheduler.Authentication.Service.Models;
using FactoryScheduler.Authentication.Service.Processors;
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
    private readonly IWorkStationProcessor _workStationProcessor;

    public WorkStationController(IMongoBaseRepository<WorkArea> workAreaRepository,
                                IMongoBaseRepository<WorkStation> workStationRepository,
                                UserManager<FactorySchedulerUser> userManager,
                                IWorkStationProcessor workStationProcessor)
    {
      _workAreaRepository = workAreaRepository;
      _workStationRepository = workStationRepository;
      _userManager = userManager;
      _workStationProcessor = workStationProcessor;
    }


    [HttpPost("workStationsByWorkAreas")]
    public async Task<ActionResult<IEnumerable<WorkStationsByWorkAreaModel>>> GetWorkStationsByWorkAreasAsync([FromBody] Guid[] workAreaIds)
    {
      var workStations = await _workStationRepository.GetAllAsync();
      var workAreas = await _workAreaRepository.GetAllAsync(p => workAreaIds.Contains(p.Id));

      var workStationsWorkAreaModel = workAreas.Select(workArea =>
      {
        var workAreaWorkStationDtos = workStations.Where(p => p.WorkAreaId == workArea.Id)?.Select(workStation =>
              {
                var workStationUsers = _userManager.Users.Where(p => p.AssignedWorkStationId == workStation.Id).AsWorkStationUsers();
                return workStation.AsDto(workArea.Name, workArea.Description, workStationUsers);
              }).ToArray();
        return new WorkStationsByWorkAreaModel
        {
          WorkAreaId = workArea.Id,
          WorkStationDtos = workAreaWorkStationDtos
        };
      });
      return Ok(workStationsWorkAreaModel);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WorkStationDto>> GetWorkStationByIdAsync([FromRoute] Guid id)
    {
      var workStation = await _workStationRepository.GetOneAsync(id);
      if (workStation == null)
      {
        return NotFound();
      }
      var workArea = await _workAreaRepository.GetFirstOrDefaultAsync(p => p.Id == workStation.WorkAreaId);
      if (workArea == null)
        return NotFound();

      var workStationUsers = _userManager.Users.Where(p => p.AssignedWorkStationId == id).AsWorkStationUsers();


      return workStation.AsDto(workArea.Name, workArea.Description, workStationUsers);
    }

    [HttpGet("WorkArea/{id}")]
    public async Task<ActionResult<IEnumerable<WorkStationDto>>> GetWorkStationsByWorkAreaIdAsync([FromRoute] Guid id)
    {
      var workArea = await _workAreaRepository.GetOneAsync(id);
      if (workArea == null)
      {
        return NotFound();
      }
      var workStations = await _workStationRepository.GetAllAsync(p => p.WorkAreaId == workArea.Id);
      if (workArea == null)
        return NotFound();

      var workStationUsers = _userManager.Users.Where(p => p.AssignedWorkStationId == id).AsWorkStationUsers();

      return workStations.Select(p => p.AsDto(workArea.Name, workArea.Description, workStationUsers)).ToArray();
    }

    [HttpPost]
    public async Task<ActionResult<WorkAreaDto>> AddWorkStationAsync([FromBody] CreateWorkStationDto createWorkStationDto)
    {
      var workStation = await _workStationProcessor.GetNewWorkStationFromWorkStationDtoAsync(createWorkStationDto);
      await _workStationRepository.CreateAsync(workStation);

      return CreatedAtAction(nameof(GetWorkStationByIdAsync), new { id = workStation.Id }, workStation);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateWorkStationAsync([FromRoute] Guid id, [FromBody] UpdateWorkStationDto updateWorkStationDto)
    {
      var workStation = await _workStationRepository.GetOneAsync(id);
      if (workStation == null)
      {
        return NotFound();
      }
      await _workStationProcessor.UpdateWorkstationFromUpdateWorkStationDtoAsync(updateWorkStationDto, workStation);

      return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWorkStationAsync([FromRoute] Guid id)
    {
      var item = await _workStationRepository.GetOneAsync(id);
      if (item == null)
      {
        return NotFound();
      }
      await _workStationRepository.RemoveAsync(id);

      return NoContent();
    }
  }
}
