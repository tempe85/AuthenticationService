using System;
using System.Collections.Generic;
using IdentityServer4.Models;

namespace FactoryScheduler.Authentication.Service.Settings
{
    public class IdentityServerSettings
    {
        //Different kinds of access to resources granted to the clients
        public IReadOnlyCollection<ApiScope> ApiScopes { get; set; } = Array.Empty<ApiScope>();

        //All the clients that have access to the microservice
        public IReadOnlyCollection<Client> Clients { get; set; } = Array.Empty<Client>();

        public IReadOnlyCollection<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId()
            };
    }
}