using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FactoryScheduler.Authentication.Service.Entities;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace FactoryScheduler.Authentication.Service.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<FactorySchedulerUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        private readonly IIdentityServerInteractionService _identityServerInteractionService;

        public LogoutModel(SignInManager<FactorySchedulerUser> signInManager,
                           IIdentityServerInteractionService identityServerInteractionService,
                           ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
            _identityServerInteractionService = identityServerInteractionService;
        }

        public async Task<IActionResult> OnGetAsync(string userId)
        {
            var context = await _identityServerInteractionService.GetLogoutContextAsync(userId);
            // if (context?.ShowSignoutPrompt == false)
            // {
            return await this.OnPost("http://localhost:3000/"); //(context.PostLogoutRedirectUri);
                                                                // }

            //return Page();
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToPage();
            }
        }
    }
}
