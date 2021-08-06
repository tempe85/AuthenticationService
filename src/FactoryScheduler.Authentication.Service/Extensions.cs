using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FactoryScheduler.Authentication.Service.Dtos;
using FactoryScheduler.Authentication.Service.Entities;
using FactoryScheduler.Authentication.Service.Interfaces;

namespace FactoryScheduler.Authentication.Service
{
    public static class Extensions
    {
        public static WorkBuildingDto AsDto(this WorkBuilding workBuilding) =>
                new WorkBuildingDto(Id: workBuilding.Id,
                                Name: workBuilding.Name,
                                Description: workBuilding.Description,
                                CreatedDate: workBuilding.CreatedDate);


        public static WorkAreaDto AsDto(this WorkArea workArea, string buildingName, string buildingDescription) =>
                new WorkAreaDto(Id: workArea.Id,
                                BuildingName: buildingName,
                                BuildingDescription: buildingDescription,
                                Name: workArea.Name,
                                Description: workArea.Description,
                                CreatedDate: workArea.CreatedDate);


        public static FactorySchedulerUserDto AsDto(this FactorySchedulerUser factorySchedulerUser) =>
                new FactorySchedulerUserDto(Id: factorySchedulerUser.Id,
                                            Username: factorySchedulerUser.UserName,
                                            Email: factorySchedulerUser.Email,
                                            CreatedDate: factorySchedulerUser.CreatedOn);

        public static WorkStationDto AsDto(this WorkStation workStation, string workAreaName, string workAreaDescription, WorkStationUser[] workStationUsers) =>
                new WorkStationDto(Id: workStation.Id, Name: workStation.Name, Description: workStation.Description,
                                    WorkStationType: workStation.WorkStationType, WorkAreaName: workAreaName,
                                    WorkAreaPosition: workStation.WorkAreaPosition, WorkerCapacity: workStation.WorkerCapacity,
                                    WorkAreaDescription: workAreaDescription, CreatedDate: workStation.CreatedDate,
                                    WorkStationUsers: workStationUsers);

        public async static Task<IReadOnlyCollection<WorkStation>> GetWorkAreaWorkStationsAsync(this WorkArea workArea, IMongoBaseRepository<WorkStation> workStationRepository)
        {
            var workStations = await workStationRepository.GetAllAsync(p => p.Id == workArea.Id);
            return workStations;

        }

        public static WorkStationUser[] AsWorkStationUsers(this IEnumerable<FactorySchedulerUser> factorySchedulerUsers) =>
            factorySchedulerUsers.Select(p => new WorkStationUser(p.Id, p.FirstName, p.LastName)).ToArray();

    }
}