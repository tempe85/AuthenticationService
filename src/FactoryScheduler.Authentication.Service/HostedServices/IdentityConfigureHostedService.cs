using System.Threading;
using System.Threading.Tasks;
using FactoryScheduler.Authentication.Service.Entities;
using FactoryScheduler.Authentication.Service.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace FactoryScheduler.Authentication.Service.HostedServices
{
    public class IdentityConfigureHostedService : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IdentitySettings _identitySettings;

        public IdentityConfigureHostedService(IServiceScopeFactory serviceScopeFactory, IOptions<IdentitySettings> identityOptions)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _identitySettings = identityOptions.Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<FactorySchedulerRole>>();
            //Use this logic to create endpoints for directly creating a user
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<FactorySchedulerUser>>();

            await CreateRoleIfDoesNotExistAsync(Roles.Admin, roleManager);
            await CreateRoleIfDoesNotExistAsync(Roles.FactorySchedulerUser, roleManager);
            await CreateRoleIfDoesNotExistAsync(Roles.FactorySchedulerPlanner, roleManager);

            var adminUser = await userManager.FindByEmailAsync(_identitySettings.AdminEmail);
            if (adminUser == null)
            {
                adminUser = new FactorySchedulerUser
                {
                    UserName = _identitySettings.AdminEmail,
                    Email = _identitySettings.AdminEmail,
                    FirstName = "Admin",
                    LastName = "Admin",
                    AssignedWorkStationId = null
                };
                await userManager.CreateAsync(adminUser, _identitySettings.AdminPassword);
                await userManager.AddToRoleAsync(adminUser, Roles.Admin);
            }

        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private static async Task CreateRoleIfDoesNotExistAsync(string role, RoleManager<FactorySchedulerRole> roleManager)
        {
            var exists = await roleManager.RoleExistsAsync(role);

            if (!exists)
            {
                await roleManager.CreateAsync(new FactorySchedulerRole { Name = role });
            }
        }
    }
}