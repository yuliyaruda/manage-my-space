using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManageMySpace.ActivityService.DAL.Interfaces;
using ManageMySpace.Common.EF;
using ManageMySpace.Common.EF.Models;
using Microsoft.EntityFrameworkCore;

namespace ManageMySpace.ActivityService.DAL
{
    public class RoomRepository : IRoomRepository
    {
        private readonly ManageMySpaceContext _database;

        public RoomRepository(ManageMySpaceContext database)
        {
            _database = database;
        }

        public async Task AddAsync(Room room)
        {
            await _database.Rooms.AddAsync(room);
            await _database.SaveChangesAsync();
        }

        public async Task DeleteAsync(string roomNumber)
        {
            var roomModel = await _database.Rooms.FirstOrDefaultAsync(r => r.RoomNumber == roomNumber);
            _database.Rooms.Remove(roomModel);
            await _database.SaveChangesAsync();
        }

        public async Task<IList<Room>> GetAllAsync()
        {
            var rooms = await _database.Rooms.ToListAsync();
            return rooms;
        }

        public async Task<IList<Room>> GetAvailableRoomsAsync(DateTime dateTime, int durationInMinutes, int capacity, bool hasProjector, bool isComputerClass)
        {
            var rooms = await _database.Rooms.Where(r => 
                                        r.Capacity > capacity &&
                                        (!hasProjector || r.HasProjector) &&
                                        (!isComputerClass || r.IsComputerClass))
                .ToListAsync();

            var reservedRoomIds = (from room in rooms
                                  join reservation in _database.Reservations on room.Id equals reservation.RoomId
                                  where room.Capacity > capacity &&
                                        (!hasProjector || room.HasProjector) &&
                                        (!isComputerClass || room.IsComputerClass) && 
                                        (reservation.StartDateTime <= dateTime && reservation.StartDateTime.AddMinutes(reservation.DurationInMinutes) >= dateTime)
                                      || (dateTime <= reservation.StartDateTime && dateTime.AddMinutes(durationInMinutes) >= reservation.StartDateTime)
                                  select room.Id).ToList();
            
            var result = rooms.Where(r => !reservedRoomIds.Contains(r.Id))
                .Select(r =>
                {
                    r.Reservations = null;
                    return r;
                });
            return result.ToList();
        }

        public async Task<Room> GetRoomAsync(string roomNumber)
        {
            var room = await _database.Rooms.FirstOrDefaultAsync(r=>r.RoomNumber == roomNumber);
            return room;
        }

        public async Task UpdateAsync(Room room)
        {
            var roomToUpdate = await _database.Rooms.FirstOrDefaultAsync(r => r.Id == room.Id);
            
            roomToUpdate.HasProjector = room.HasProjector;
            roomToUpdate.IsComputerClass = room.IsComputerClass;
            roomToUpdate.Capacity = room.Capacity;
            roomToUpdate.RoomNumber = room.RoomNumber;

            await _database.SaveChangesAsync();
        }

        public async Task<bool> CheckAvailableRoomsAsync(DateTime dateTime, int durationInMinutes, Guid roomId)
        {
            var rooms = await _database.Rooms.ToListAsync();
            var reservedRoomIds = from room in rooms
                join reservation in _database.Reservations on room.Id equals reservation.RoomId
                where (reservation.StartDateTime <= dateTime && reservation.StartDateTime.AddMinutes(reservation.DurationInMinutes) >= dateTime)
                      || (dateTime <= reservation.StartDateTime && dateTime.AddMinutes(durationInMinutes) >= reservation.StartDateTime)
                select room.Id;
            return rooms.Where(r => !reservedRoomIds.Contains(r.Id)).Any(r => r.Id == roomId);
        }
    }
}
