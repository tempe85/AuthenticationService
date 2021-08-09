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
                new IdentityResources.Profile(),
                new IdentityResource("roles", new[]{"role"}), //this makes sure that the role is included in the token that the identity server sends back
                new IdentityResource("user_data", new[]{"AssignedWorkStationId", "LastName", "FirstName"})
            };
    }
}