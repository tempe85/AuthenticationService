using System;
using System.ComponentModel.DataAnnotations;
using FactoryScheduler.Authentication.Service.Enums;

namespace FactoryScheduler.Authentication.Service.Dtos
{
  public record WorkBuildingDto(
      Guid Id,
      string Name,
      string Description,
      DateTimeOffset CreatedDate);

  public record CreateWorkBuildingDto(
      [Required] string Name,
      string Description);

  public record UpdateWorkBuildingDto(
      [Required] string Name,
      string Description);


}