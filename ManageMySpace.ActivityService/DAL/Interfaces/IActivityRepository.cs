using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ManageMySpace.Common.Commands.ActivityCommands;
using ManageMySpace.Common.EF.Models;

namespace ManageMySpace.ActivityService.DAL.Interfaces
{
    public interface IActivityRepository
    {
        Task AddAsync(Activity activity, List<Reservation> reservations, string email);
        Task DeleteAsync(Activity activity);
        Task<Activity> GetActivityAsync(string name);
        Task<Activity> GetActivityAsync(Guid id);
        Task<IList<Activity>> GetAllActivitiesAsync();
        Task AddVisitorAsync(Activity activity, string userEmail);
        Task DeleteVisitorAsync(Activity activity, string userEmail);
        Task<bool> HasPermissionToDeleteEvent(Activity activity, string userEmail);
    }
}
