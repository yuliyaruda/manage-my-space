using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ManageMySpace.Common.Commands.UserCommands;
using ManageMySpace.Common.Events.UserEvents;
using ManageMySpace.Common.Exceptions;
using ManageMySpace.UserService.API.Models;
using ManageMySpace.UserService.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RawRabbit;
using Serilog;

namespace ManageMySpace.UserService.API
{
    [Route("")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IBusClient _busClient;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IBusClient busClient, IMapper mapper)
        {
            _userService = userService;
            _busClient = busClient;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUser command)
        {
            return new JsonResult(await _userService.LoginAsync(command.Email, command.Password));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUser request)
        {
            try
            {
                await _userService.RegisterAsync(request.Email, request.Name, request.Password, request.LastName);
                
                var command = new UserCreated(request.Email, request.Name);
                Log.ForContext(nameof(UserCreated), command, true).Information("User has been successfully registered. Message is publishing");
                await _busClient.PublishAsync(command);
                Log.ForContext(nameof(UserCreated), command, true).Information("Message about user registration is published.");
                
                return Accepted();
            }
            catch(ManageMySpaceException exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost("block/user")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> BlockUser(BanUser request)
        {
            try
            {
                await _userService.BanUserAsync(request.InvokerId, request.UserEmail);
                var user = await _userService.GetAsync(request.UserEmail);

                var command = new UserBanned(request.InvokerId, request.UserEmail, user.Name);
                Log.ForContext(nameof(UserBanned), command, true).Information("User has been blocked. Message is publishing");
                await _busClient.PublishAsync(command);
                Log.ForContext(nameof(UserBanned), command, true).Information("Message about user block is published.");
                
                return Accepted();
            }
            catch (ManageMySpaceException exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost("unblock/user")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnblockUser(BanUser request)
        {
            try
            {
                await _userService.UnblockUserAsync(request.InvokerId, request.UserEmail);
                
                var user = await _userService.GetAsync(request.UserEmail);
                var command = new UserUnblocked(request.InvokerId, request.UserEmail, user.Name);
                Log.ForContext(nameof(UserUnblocked), command, true).Information("User has been unblocked. Message is publishing");
                await _busClient.PublishAsync(command);
                Log.ForContext(nameof(UserUnblocked), command, true).Information("Message about user unblock is published.");
                
                return Accepted();
            }
            catch (ManageMySpaceException exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("user/info")]
        public async Task<IActionResult> Get(string email)
        {
            var userInfo = await _userService.GetAsync(email);
            return new JsonResult(_mapper.Map<UserInfoResponse>(userInfo));
        }

        [HttpGet("all/users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var userInfo = await _userService.GetAllAsync();
            return new JsonResult(_mapper.Map<List<UserInfoResponse>>(userInfo));
        }

        [HttpPost("assign/role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveRole(AssignRoleToUser request)
        {
            try
            {
                var invokerEEmail = HttpContext.User.Identity.Name;
                await _userService.AssignRoleToUser(invokerEEmail, request.RoleName, request.UserEmail);
               
                var user = await _userService.GetAsync(request.UserEmail);
                var command = new RoleAssignedToUser(Guid.NewGuid(), request.UserEmail, request.RoleName, user.Name);
                Log.ForContext(nameof(RoleAssignedToUser), command, true).Information("User's role has been successfully changed. Message is publishing");
                await _busClient.PublishAsync(command);
                Log.ForContext(nameof(RoleAssignedToUser), command, true).Information("Message about user's role change is published.");
                
                return Accepted();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}