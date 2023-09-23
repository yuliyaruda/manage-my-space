using System;
using System.Threading.Tasks;
using AutoMapper;
using ManageMySpace.ActivityService.API.Models;
using ManageMySpace.ActivityService.BLL.Interfaces;
using ManageMySpace.Common.Commands.ActivityCommands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RawRabbit;

namespace ManageMySpace.ActivityService.API.Controllers
{
    [Route("")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private IRoomService _roomService;
        private IBusClient _busClient;
        private IMapper _mapper;
        public RoomController(IRoomService roomService, IBusClient busClient, IMapper mapper)
        {
            _roomService = roomService;
            _busClient = busClient;
            _mapper = mapper;
        }

        [HttpPost("/create/room")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRoom(CreateRoomRequest request)
        {
            try
            {
                await _roomService.AddAsync(_mapper.Map<CreateRoom>(request));
                return Accepted();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("/update/room")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRoom(CreateRoomRequest request)
        {
            try
            {
                await _roomService.UpdateAsync(_mapper.Map<CreateRoom>(request));
                return Accepted();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("/delete/room")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRoom([FromBody]DeleteRoomClient request)
        {
            try
            {
                await _roomService.DeleteAsync(request.RoomNumber);
                return Accepted();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("/room")]
        public async Task<IActionResult> GetRooms()
        {
            try
            {
                return Ok(await _roomService.GetAll());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("/rooms/by/filter")]
        public async Task<IActionResult> GetAvailableRooms(RoomFilters roomFilter)
        {
            try
            {
                return Ok(await _roomService.GetAvailableRooms(roomFilter.DateTime, roomFilter.Duration, roomFilter.Capacity, roomFilter.HasProjector, roomFilter.IsComputerClass));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}