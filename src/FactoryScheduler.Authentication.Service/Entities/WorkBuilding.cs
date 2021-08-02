using System;
using FactoryScheduler.Authentication.Service.Interfaces;

namespace FactoryScheduler.Authentication.Service.Entities
{
    public class WorkBuilding : IMongoEntity
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreatedDate { get; init; }
    }
}