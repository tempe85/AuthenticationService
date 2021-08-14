using System;
using System.Linq;
using System.Threading.Tasks;
using FactoryScheduler.Authentication.Service.Dtos;
using FactoryScheduler.Authentication.Service.Entities;
using FactoryScheduler.Authentication.Service.Interfaces;
using IdentityServer4.Extensions;

namespace FactoryScheduler.Authentication.Service.Processors
{

  public class WorkStationProcessor : IWorkStationProcessor
  {
    private readonly IMongoBaseRepository<WorkArea> _workAreaRepository;
    private readonly IMongoBaseRepository<WorkStation> _workStationRepository;

    public WorkStationProcessor(IMongoBaseRepository<WorkArea> workAreaRepository,
                                IMongoBaseRepository<WorkStation> workStationRepository)
    {
      _workAreaRepository = workAreaRepository;
      _workStationRepository = workStationRepository;
    }

    public async Task<WorkStation> GetNewWorkStationFromWorkStationDtoAsync(CreateWorkStationDto createWorkStationDto)
    {
      var workArea = await _workAreaRepository.GetOneAsync(createWorkStationDto.WorkAreaId);
      if (workArea == null)
      {
        throw new Exception($"Unable to find work area: {createWorkStationDto.WorkAreaId} for created work station");
      }
      var workAreaWorkStations = await workArea.GetWorkAreaWorkStationsAsync(_workStationRepository);

      var maxOperationPosition = workAreaWorkStations.IsNullOrEmpty() ? -1 : workAreaWorkStations.Select(p => p.WorkAreaPosition).ToArray().Max();

      return new WorkStation
      {
        CreatedDate = DateTimeOffset.UtcNow,
        Description = createWorkStationDto.Description,
        Id = Guid.NewGuid(),
        Name = createWorkStationDto.Name,
        WorkAreaId = workArea.Id,
        WorkAreaPosition = maxOperationPosition + 1,
        WorkerCapacity = createWorkStationDto.WorkerCapacity,
        WorkStationType = createWorkStationDto.WorkStationType,
      };
    }

    public async Task UpdateWorkstationFromUpdateWorkStationDtoAsync(UpdateWorkStationDto updateWorkStationDto, WorkStation workStation)
    {
      workStation.Description = updateWorkStationDto.Description;
      workStation.Name = updateWorkStationDto.Name;
      workStation.isDeleted = updateWorkStationDto.isDeleted;

      await _workStationRepository.UpdateAsync(workStation);
    }
  }
}