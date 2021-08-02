using System;
using System.ComponentModel.DataAnnotations;

namespace FactoryScheduler.Authentication.Service.Dtos
{
    public record BuildingDto(Guid Id, string Name, string Description, DateTimeOffset CreatedDate);

    public record CreateBuildingDto([Required] string Name, string Description);

    public record UpdateBuildingDto([Required] string Name, string Description);

    public record WorkAreaDto(Guid Id, string BuildingName, string BuildingDescription, string Name, string Description, DateTimeOffset CreatedDate);

    public record CreateWorkAreaDto([Required] string Name, [Required] Guid WorkBuildingId, string Description);

    public record UpdateWorkAreaDto([Required] string Name, string Description);

}