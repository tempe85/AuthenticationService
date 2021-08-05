using System;
using AspNetCore.Identity.MongoDbCore.Models;
using FactoryScheduler.Authentication.Service.Interfaces;
using MongoDbGenericRepository.Attributes;

namespace FactoryScheduler.Authentication.Service.Entities
{

    [CollectionName("FactorySchedulerUsers")]
    public class FactorySchedulerUser : MongoIdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}