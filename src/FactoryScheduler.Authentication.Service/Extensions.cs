using FactoryScheduler.Authentication.Service.Dtos;
using FactoryScheduler.Authentication.Service.Entities;

namespace FactoryScheduler.Authentication.Service
{
    public static class Extensions
    {
        public static BuildingDto AsDto(this WorkBuilding workBuilding)
        {
            return new BuildingDto(Id: workBuilding.Id, Name: workBuilding.Name, Description: workBuilding.Description, CreatedDate: workBuilding.CreatedDate);
        }

        public static WorkAreaDto AsDto(this WorkArea workArea, string buildingName, string buildingDescription)
        {
            return new WorkAreaDto(Id: workArea.Id,
                                   BuildingName: buildingName,
                                   BuildingDescription: buildingDescription,
                                   Name: workArea.Name,
                                   Description: workArea.Description,
                                   CreatedDate: workArea.CreatedDate);
        }
    }
}