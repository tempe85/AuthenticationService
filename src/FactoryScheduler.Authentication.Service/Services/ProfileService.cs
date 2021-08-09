using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FactoryScheduler.Authentication.Service.Entities;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace FactoryScheduler.Authentication.Service
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<FactorySchedulerUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<FactorySchedulerUser> _claimsFactory;

        protected readonly ILogger<ProfileService> _logger;

        public ProfileService(UserManager<FactorySchedulerUser> userManager, IUserClaimsPrincipalFactory<FactorySchedulerUser> claimsFactory, ILogger<ProfileService> logger)
        {
            _claimsFactory = claimsFactory;
            _userManager = userManager;
            _logger = logger;

        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            context.LogProfileRequest(_logger);
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            var principal = await _claimsFactory.CreateAsync(user);

            var claims = principal.Claims.ToList();
            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();
            claims.Add(new Claim("AssignedWorkStationId", user.AssignedWorkStationId?.ToString() ?? string.Empty));
            claims.Add(new Claim("LastName", user.LastName ?? string.Empty));
            claims.Add(new Claim("FirstName", user.FirstName ?? string.Empty));
            context.AddRequestedClaims(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            _logger.LogDebug("IsActive called from: {caller}", context.Caller);
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}