using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ManageMySpace.ActivityService.API.Models;
using ManageMySpace.ActivityService.BLL.Interfaces;
using ManageMySpace.Common.Commands.ActivityCommands;
using ManageMySpace.Common.Events.ActivityEvents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RawRabbit;
using Serilog;

namespace ManageMySpace.ActivityService.API.Controllers
{
    [Route("")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private IActivityService _activityService;
        private IBusClient _busClient;
        private readonly IMapper _mapper;
        public ActivityController(IActivityService activityService, IBusClient busClient, IMapper mapper)
        {
            _activityService = activityService;
            _busClient = busClient;
            _mapper = mapper;
        }

        [HttpGet("/activity/{id}")]
        public async Task<IActionResult> GetActivity(Guid id)
        {
            var userId = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Properties.Values.Contains(JwtRegisteredClaimNames.NameId)).Value);
            var activity = await _activityService.GetActivityAsync(id);
            var activityResponce = _mapper.Map<ActivityResponseModel>(activity);
            activityResponce.IsVisitor = activity.Visitors.Any(v => v.UserId == userId);
            activityResponce.IsOrganizator = activity.Organizators.Any(v => v.UserId == userId); ;
            return new JsonResult(activityResponce);
        }

        [HttpGet("/activities")]
        public async Task<IActionResult> GetActivities()
        {
            var activities = await _activityService.GetActivitiesAsync();
            return new JsonResult(_mapper.Map<List<ActivityResponseModel>>(activities));
        }

        [HttpPost("/vistit/activity")]
        [Authorize]
        public async Task<IActionResult> VisitActivity(VisitActivityRequest request)
        {
            try
            {
                var visitorEmail = HttpContext.User.Identity.Name;
                await _activityService.VisitActivity(request.EventId, visitorEmail);
                var activity = await _activityService.GetActivityAsync(request.EventId);

                var command = new ActivitySubscribed(visitorEmail, activity.Name);
                Log.ForContext(nameof(ActivitySubscribed), command, true).Information("User has been subscribed to activity. Message is publishing");
                await _busClient.PublishAsync(command);
                Log.ForContext(nameof(ActivitySubscribed), command, true).Information("Message about the subscription to activity is published.");

                return Accepted();
            }
            catch(Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("/cancel/vistit/activity")]
        [Authorize]
        public async Task<IActionResult> CancelVisitingActivity(CancelVisitActivityRequest request)
        {
            try
            {
                var visitorEmail = HttpContext.User.Identity.Name;
                await _activityService.CancelVisitActivity(request.EventId, visitorEmail);
                return Accepted();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("/create/activity")]
        [Authorize(Roles = "Admin, Student")]
        public async Task<IActionResult> CreateActivity(CreateActivityRequest activityRequest)
        {
            try
            {
                var visitorEmail = HttpContext.User.Identity.Name;
                await _activityService.AddAsync(activityRequest, visitorEmail);

                var command = new ActivityCreated(visitorEmail, activityRequest.Name);
                Log.ForContext(nameof(ActivityCreated), command, true).Information("Activity has been created. Message is publishing");
                await _busClient.PublishAsync(command);
                Log.ForContext(nameof(ActivityCreated), command, true).Information("Message about the cration of the activity is published.");

                return Accepted();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("/delete/activity")]
        [Authorize(Roles = "Admin, Student")]
        public async Task<IActionResult> DeleteActivity(DeleteActivityRequest request)
        {
            try
            {
                var visitorEmail = HttpContext.User.Identity.Name;
                var @event = await _activityService.GetActivityAsync(request.EventId);
                var eventName = await _activityService.DeleteActivityAsync(request.EventId, visitorEmail);
                
                var command = new ActivityCanceled(@event.Visitors.Select(v => v.User.Email).ToList(), eventName,
                    @event.StartDate);

                Log.ForContext(nameof(ActivityCanceled), command, true).Information("Activity has been canceled. Message is publishing");
                await _busClient.PublishAsync(command);
                Log.ForContext(nameof(ActivityCanceled), command, true).Information("Message about the cancellation of the activity is published.");

                return Accepted();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}