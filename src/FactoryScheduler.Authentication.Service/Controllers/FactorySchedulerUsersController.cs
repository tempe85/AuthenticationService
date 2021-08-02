using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FactoryScheduler.Authentication.Service.Dtos;
using FactoryScheduler.Authentication.Service.Entities;
using FactoryScheduler.Authentication.Service.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static IdentityServer4.IdentityServerConstants;

namespace FactoryScheduler.Authentication.Service.Controllers
{
    [ApiController]
    [Route("Users")]
    [Authorize(Roles = Roles.Admin, Policy = LocalApi.PolicyName)]
    public class FactorySchedulerUsersController : ControllerBase
    {
        private readonly UserManager<FactorySchedulerUser> _userManager;

        public FactorySchedulerUsersController(UserManager<FactorySchedulerUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public ActionResult<IEnumerable<FactorySchedulerUserDto>> GetUsers()
        {
            var users = _userManager.Users
                        .ToList()
                        .Select(p => p.AsDto());
            return Ok(users);
        }

        //Adding this type of user requires no auth
        [HttpPost]
        public async Task<ActionResult<FactorySchedulerUserDto>> AddFactorySchedulerUser([FromBody] AddFactorySchedulerUserDto addFactorySchedulerUserDto)
        {
            var existingUser = await _userManager.FindByEmailAsync(addFactorySchedulerUserDto.Email);
            if (existingUser != null)
            {
                //Need a better return error message (to explain that user with that email already exists)
                return Forbid();
            }
            var createdUser = new FactorySchedulerUser
            {
                Email = addFactorySchedulerUserDto.Email,
                UserName = addFactorySchedulerUserDto.Email
            };
            await _userManager.CreateAsync(createdUser, addFactorySchedulerUserDto.Password);
            await _userManager.AddToRoleAsync(createdUser, Roles.FactorySchedulerUser);

            return CreatedAtAction(nameof(GetUserByIdAsync), new { id = createdUser.Id }, createdUser);
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<FactorySchedulerUserDto>> GetUserByIdAsync([FromRoute] Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }
            return user.AsDto();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAsync([FromRoute] Guid id, UpdateFactorySchedulerUserDto updateFactorySchedulerUserDto)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }
            user.Email = updateFactorySchedulerUserDto.Email;

            await _userManager.UpdateAsync(user);

            return NoContent();
        }

        [HttpPut("roles/{id}")]
        public async Task<ActionResult<FactorySchedulerUserDto>> UpdateUserRoleAsync([FromRoute] Guid id, [FromBody] FactorySchedulerRoleType[] roles)
        {

            var existingUser = await _userManager.FindByIdAsync(id.ToString());
            if (existingUser == null)
            {
                return NotFound();
            }
            var currentRoles = existingUser.Roles;


            if (roles.Any(p => p == FactorySchedulerRoleType.Admin))
            {
                await _userManager.AddToRoleAsync(existingUser, Roles.Admin);
            }
            if (roles.Any(p => p == FactorySchedulerRoleType.Planner))
            {
                await _userManager.AddToRoleAsync(existingUser, Roles.FactorySchedulerPlanner);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            await _userManager.DeleteAsync(user);

            return NoContent();
        }
    }
}