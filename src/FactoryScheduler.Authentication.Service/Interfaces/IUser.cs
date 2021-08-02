using System;

namespace FactoryScheduler.Authentication.Service.Interfaces
{
    public interface IUser
    {
        Guid Id { get; set; }
        string LastName { get; set; }
        string FirstName { get; set; }
        string Email { get; set; }
    }
}