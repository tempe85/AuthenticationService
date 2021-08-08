using System;
using System.ComponentModel.DataAnnotations;

namespace FactoryScheduler.Authentication.Service.Dtos
{
    public record FactorySchedulerUserDto(
        Guid Id,
        string Username,
        string Email,
        Guid? AssignedWorkStationId,
        string FirstName,
        string LastName,
        DateTimeOffset CreatedDate);
    public record AddFactorySchedulerUserDto([Required][EmailAddress] string Email, [Required] string Password);
    public record UpdateFactorySchedulerUserDto([Required][EmailAddress] string Email);

}