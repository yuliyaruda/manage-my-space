using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ManageMySpace.Common.Commands.ActivityCommands;
using ManageMySpace.Common.EF.Models;

namespace ManageMySpace.ActivityService.BLL.Interfaces
{
    public interface IActivityService
    {
        Task<Activity> GetActivityAsync(string name);
        Task<IList<Activity>> GetActivitiesAsync();
        Task VisitActivity(Guid id, string userEmail);
        Task CancelVisitActivity(Guid id, string userEmail);
        Task<string> DeleteActivityAsync(Guid id, string userEmail);
        Task AddAsync(CreateActivityRequest activityRequest, string email);
        Task<Activity> GetActivityAsync(Guid id);
    }
}
