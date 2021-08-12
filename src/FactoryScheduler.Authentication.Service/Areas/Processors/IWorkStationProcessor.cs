using System.Threading.Tasks;
using FactoryScheduler.Authentication.Service.Dtos;
using FactoryScheduler.Authentication.Service.Entities;

namespace FactoryScheduler.Authentication.Service.Processors
{
    public interface IWorkStationProcessor
    {
        Task<WorkStation> GetNewWorkStationFromWorkStationDtoAsync(CreateWorkStationDto createWorkStationDto);

        Task UpdateWorkstationFromUpdateWorkStationDtoAsync(UpdateWorkStationDto updateWorkStationDto, WorkStation workStation);
    }
}