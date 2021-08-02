using System;
using System.ComponentModel.DataAnnotations;

namespace FactoryScheduler.Authentication.Service.Dtos
{
    public record FactorySchedulerUserDto(Guid Id, string Username, string Email, DateTimeOffset CreatedDate);
    public record UpdateFactorySchedulerUserDto([Required][EmailAddress] string Email);

}