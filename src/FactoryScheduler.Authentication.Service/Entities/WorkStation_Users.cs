using System;
using System.Linq;
using FactoryScheduler.Authentication.Service.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FactoryScheduler.Authentication.Service.Entities
{
    public class WorkStation_Users : IMongoEntity
    {
        public Guid Id { get; init; }
        //Need to add unique key constraint for workstationid
        public Guid WorkStationId { get; init; }
        public Guid[] UserIds { get; set; }
    }
}