using System;

namespace FactoryScheduler.Authentication.Service.Entities
{
    public class User 
    {
        public Guid Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
    }
}