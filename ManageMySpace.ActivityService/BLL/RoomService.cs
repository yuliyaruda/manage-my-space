using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ManageMySpace.ActivityService.BLL.Interfaces;
using ManageMySpace.ActivityService.DAL.Interfaces;
using ManageMySpace.Common.Commands.ActivityCommands;
using ManageMySpace.Common.EF.Models;
using ManageMySpace.Common.Exceptions;

namespace ManageMySpace.ActivityService.BLL
{
    public class RoomService : IRoomService
    {
        public IRoomRepository _roomRepository;
        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task AddAsync(CreateRoom room)
        {
            if (room == null)
            {
                throw new ManageMySpaceException("room_could_not_be_null");
            }
            await _roomRepository.AddAsync(new Room 
            {
                Capacity = room.Capacity,
                HasProjector = room.HasProjector,
                IsComputerClass = room.IsComputerClass,
                RoomNumber = room.RoomNumber
            });
        }

        public async Task DeleteAsync(string roomNumber)
        {
            if (string.IsNullOrEmpty(roomNumber))
            {
                throw new ManageMySpaceException("room_could_not_be_null");
            }
            await _roomRepository.DeleteAsync(roomNumber);
        }

        public async Task<IList<Room>> GetAll()
        {
            return await _roomRepository.GetAllAsync();
        }

        public async Task<IList<Room>> GetAvailableRooms(DateTime dateTime, int durationInMinutes, int capacity, bool hasProjector, bool isComputerClass)
        {
            return await _roomRepository.GetAvailableRoomsAsync(dateTime, durationInMinutes, capacity, hasProjector, isComputerClass);
        }

        public async Task<Room> GetRoom(string roomNumber)
        {
            if (string.IsNullOrEmpty(roomNumber))
            {
                throw new ManageMySpaceException("room_could_not_be_null");
            }
            return await _roomRepository.GetRoomAsync(roomNumber);
        }

        public async Task UpdateAsync(CreateRoom room)
        {
            if (room == null)
            {
                throw new ManageMySpaceException("room_could_not_be_null");
            }

            await _roomRepository.UpdateAsync(new Room
            {
                Capacity = room.Capacity,
                HasProjector = room.HasProjector,
                IsComputerClass = room.IsComputerClass,
                RoomNumber = room.RoomNumber,
            });
        }
    }
}
