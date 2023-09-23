using System;
using System.Threading.Tasks;
using ManageMySpace.UserService.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RawRabbit;

namespace ManageMySpace.UserService.API
{
    [Route("")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IBusClient _busClient;

        public RoleController(IRoleService roleService, IBusClient busClient)
        {
            _roleService = roleService;
            _busClient = busClient;
        }

        [HttpGet("/roles")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetRoles()
        {
            return new JsonResult(await _roleService.GetAllAsync());
        }

        [HttpPost("/add/role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRole(string roleName)
        {
            try
            {
                await _roleService.AddAsync(roleName);
                return Accepted();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("/remove/role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveRole(string roleName)
        {
            try
            {
                await _roleService.RemoveAsync(roleName);
                return Accepted();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}