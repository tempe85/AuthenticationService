using System;
using System.ComponentModel.DataAnnotations;
using FactoryScheduler.Authentication.Service.Enums;

namespace FactoryScheduler.Authentication.Service.Dtos
{
    //WorkArea Dtos
    public record WorkAreaDto(
        Guid Id, string BuildingName, 
        string BuildingDescription, 
        string Name, 
        string Description, 
        DateTimeOffset CreatedDate);

    public record CreateWorkAreaDto(
        [Required] string Name, 
        [Required] Guid WorkBuildingId, 
        string Description);

    public record UpdateWorkAreaDto(
        [Required] string Name, 
        string Description);
}