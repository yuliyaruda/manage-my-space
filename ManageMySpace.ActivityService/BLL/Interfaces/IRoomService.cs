using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ManageMySpace.Common.Commands.ActivityCommands;
using ManageMySpace.Common.EF.Models;

namespace ManageMySpace.ActivityService.BLL.Interfaces
{
    public interface IRoomService
    {
        Task AddAsync(CreateRoom room);
        Task UpdateAsync(CreateRoom room);
        Task DeleteAsync(string roomNumber);
        Task<Room> GetRoom(string roomNumber);
        Task<IList<Room>> GetAll();

        Task<IList<Room>> GetAvailableRooms(DateTime dateTime, int durationInMinutes, int capacity, bool hasProjector, bool isComputerClass);
    }
}
