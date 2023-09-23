using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ManageMySpace.ActivityService.BLL.Interfaces;
using ManageMySpace.ActivityService.DAL.Interfaces;
using ManageMySpace.Common.Commands.ActivityCommands;
using ManageMySpace.Common.EF.Models;

namespace ManageMySpace.ActivityService.BLL
{
    public class ActivityService : IActivityService
    {
        public IRoomRepository _roomRepository;
        public IActivityRepository _activityRepository;

        public ActivityService(IRoomRepository roomRepository, IActivityRepository activityRepository)
        {
            _roomRepository = roomRepository;
            _activityRepository = activityRepository;
        }

        public async Task AddAsync(CreateActivityRequest activityRequest, string email)
        {
            var room = await _roomRepository.GetRoomAsync(activityRequest.RoomNumber);

            if (room == null)
            {
                throw new ArgumentException();
            }

            if (!await _roomRepository.CheckAvailableRoomsAsync(activityRequest.StartDate, activityRequest.Duration, room.Id))
            {
                throw new ArgumentException();
            }

            var activityModel = new Activity
            {
                Description = activityRequest.Description,
                EndDate = activityRequest.EndDate,
                StartDate = activityRequest.StartDate,
                Name = activityRequest.Name,
                PeriodType = activityRequest.PeriodType
            };

            List<Reservation> reservations = GetReservations(activityRequest, room);

            await _activityRepository.AddAsync(activityModel, reservations, email);
        }

        public async Task CancelVisitActivity(Guid id, string userEmail)
        {
            var activity = await _activityRepository.GetActivityAsync(id);
            await _activityRepository.DeleteVisitorAsync(activity, userEmail);
        }

        public async Task<string> DeleteActivityAsync(Guid id, string userEmail)
        {
            var activityModel = await _activityRepository.GetActivityAsync(id);
            var name = activityModel.Name;
            if (await _activityRepository.HasPermissionToDeleteEvent(activityModel, userEmail))
            {
                await _activityRepository.DeleteAsync(activityModel);
            }
            return name;
        }

        public async Task<IList<Activity>> GetActivitiesAsync()
        {
            return await _activityRepository.GetAllActivitiesAsync();
        }

        public async Task<Activity> GetActivityAsync(string name)
        {
            return await _activityRepository.GetActivityAsync(name);
        }

        public async Task<Activity> GetActivityAsync(Guid id)
        {
            return await _activityRepository.GetActivityAsync(id);
        }

        public async Task VisitActivity(Guid id, string userEmail)
        {
            var activity = await _activityRepository.GetActivityAsync(id);
            await _activityRepository.AddVisitorAsync(activity, userEmail);
        }

        private List<Reservation> GetReservations(CreateActivityRequest activityRequest, Room room)
        {
            List<Reservation> reservations = new List<Reservation>();
            int index = 1;
            switch (activityRequest.PeriodType)
            {
                case 0:
                    reservations.Add(new Reservation
                    {
                        DurationInMinutes = activityRequest.Duration,
                        RoomId = room.Id,
                        StartDateTime = activityRequest.StartDate
                    }); break;

                case 1:
                    while (true)
                    {
                        var startDate = activityRequest.StartDate.AddDays(7 * index++);
                        if (startDate > activityRequest.EndDate)
                            break;
                        reservations.Add(new Reservation
                        {
                            DurationInMinutes = activityRequest.Duration,
                            RoomId = room.Id,
                            StartDateTime = startDate
                        });
                    }
                    break;
            }
            return reservations;
        }
    }
}
