using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ManageMySpace.Common.EF.Models;

namespace ManageMySpace.ActivityService.DAL.Interfaces
{
    public interface IRoomRepository
    {
        Task AddAsync(Room room);
        Task UpdateAsync(Room room);
        Task DeleteAsync(string roomNumber);
        Task<Room> GetRoomAsync(string roomNumber);
        Task<IList<Room>> GetAllAsync();
        Task<IList<Room>> GetAvailableRoomsAsync(DateTime dateTime, int durationInMinutes, int capacity, bool hasProjector, bool isComputerClass);
        Task<bool> CheckAvailableRoomsAsync(DateTime dateTime, int durationInMinutes, Guid roomId);
    }
}
