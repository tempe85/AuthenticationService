using System;
using System.ComponentModel.DataAnnotations;
using FactoryScheduler.Authentication.Service.Enums;

namespace FactoryScheduler.Authentication.Service.Dtos
{
    //Workbuilding Dtos
    public record WorkBuildingDto(Guid Id, string Name, string Description, DateTimeOffset CreatedDate);

    public record CreateWorkBuildingDto([Required] string Name, string Description);

    public record UpdateWorkBuildingDto([Required] string Name, string Description);

    //WorkArea Dtos
    public record WorkAreaDto(Guid Id, string BuildingName, string BuildingDescription, string Name, string Description, DateTimeOffset CreatedDate);

    public record CreateWorkAreaDto([Required] string Name, [Required] Guid WorkBuildingId, string Description);

    public record UpdateWorkAreaDto([Required] string Name, string Description);

    //WorkStation Dtos
    public record WorkStationDto(Guid Id,
                                 string Name,
                                 string Description,
                                 WorkStationType WorkStationType,
                                 string WorkAreaName,
                                 int WorkAreaPosition,
                                 WorkStationUser[] WorkStationUsers,
                                 int WorkerCapacity,
                                 string WorkAreaDescription,
                                 DateTimeOffset CreatedDate);

    public record CreateWorkStationDto([Required] string Name, [Required] Guid WorkAreaId, [Required] WorkStationType WorkStationType,
                                       [Required] int WorkerCapacity, string Description);

    public record UpdateWorkStationDto([Required] string Name, string Description, bool isDeleted);

    //WorkStation Users Dto
    //public record WorkStationUsersDto(Guid Id, UsersDto)

}