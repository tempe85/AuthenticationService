using System;
using System.ComponentModel.DataAnnotations;
using FactoryScheduler.Authentication.Service.Enums;

namespace FactoryScheduler.Authentication.Service.Dtos
{
  //WorkStation Dtos
  public record WorkStationDto(
      Guid Id,
      string Name,
      string Description,
      WorkStationType WorkStationType,
      string WorkAreaName,
      int WorkAreaPosition,
      WorkStationUser[] WorkStationUsers,
      int WorkerCapacity,
      string WorkAreaDescription,
      DateTimeOffset CreatedDate);

  public record CreateWorkStationDto(
      [Required] string Name,
      [Required] Guid WorkAreaId,
      [Required] WorkStationType WorkStationType,
      [Required] int WorkerCapacity, string Description);

  public record UpdateWorkStationDto(
      [Required] string Name,
      string Description,
      bool isDeleted);
}