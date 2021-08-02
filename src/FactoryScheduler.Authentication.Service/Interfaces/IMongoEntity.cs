using System;

namespace FactoryScheduler.Authentication.Service.Interfaces
{
    public interface IMongoEntity
    {
        Guid Id { get; init; }
    }
}