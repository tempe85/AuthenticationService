using FactoryScheduler.Authentication.Service.Dtos;
using FactoryScheduler.Authentication.Service.Entities;

namespace FactoryScheduler.Authentication.Service
{
    public static class Extensions
    {
        public static BuildingDto AsDto(this WorkBuilding workBuilding) =>
                new BuildingDto(Id: workBuilding.Id,
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

    }
}