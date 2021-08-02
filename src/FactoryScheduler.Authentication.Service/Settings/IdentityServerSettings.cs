using System;
using System.Collections.Generic;
using IdentityServer4.Models;

namespace FactoryScheduler.Authentication.Service.Settings
{
    public class IdentityServerSettings
    {
        //Different kinds of access to resources granted to the clients
        public IReadOnlyCollection<ApiScope> ApiScopes { get; init; }
        public IReadOnlyCollection<ApiResource> ApiResources { get; init; }

        //All the clients that have access to the microservice
        public IReadOnlyCollection<Client> Clients { get; init; }

        public IReadOnlyCollection<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
    }
}