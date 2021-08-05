using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FactoryScheduler.Authentication.Service.Entities;
using FactoryScheduler.Authentication.Service.Interfaces;
using MongoDB.Driver;

namespace FactoryScheduler.Authentication.Service.Repositories
{
    public class WorkStationRepository : MongoBaseRepository<WorkStation>
    {
        public WorkStationRepository(
            IMongoCollection<WorkStation> collection) : base(collection)
        {

        }
    }
}