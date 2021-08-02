using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FactoryScheduler.Authentication.Service.Entities;
using MongoDB.Driver;

namespace FactoryScheduler.Authentication.Service.Repositories
{
    public class WorkStationUsersRepository : MongoBaseRepository<WorkStation_Users>
    {
        public WorkStationUsersRepository(
            IMongoCollection<WorkStation_Users> collection) : base(collection)
        {

        }
    }
}