using System;
using FactoryScheduler.Authentication.Service.Enums;
using FactoryScheduler.Authentication.Service.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FactoryScheduler.Authentication.Service.Entities
{
    public class WorkStation : IMongoEntity
    {
        public Guid Id { get; init; }
        public Guid WorkAreaId { get; init; }
        public WorkStationType WorkStationType { get; init; }
        //Example: Type: Build, Description: Build 1
        public string Description { get; set; }
        public int WorkerCapacity { get; set; }
        public int WorkAreaPosition { get; set; }
        public bool isDeleted { get; set; } = false;
    }
}