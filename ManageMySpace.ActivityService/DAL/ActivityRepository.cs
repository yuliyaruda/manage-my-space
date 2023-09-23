using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManageMySpace.ActivityService.DAL.Interfaces;
using ManageMySpace.Common.Commands.ActivityCommands;
using ManageMySpace.Common.EF;
using ManageMySpace.Common.EF.Models;
using ManageMySpace.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ManageMySpace.ActivityService.DAL
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly ManageMySpaceContext _database;

        public ActivityRepository(ManageMySpaceContext database)
        {
            _database = database;
        }

        public async Task AddAsync(Activity activity, List<Reservation> reservations, string email)
        {
            var user = await _database.Users.FirstOrDefaultAsync(u => u.Email == email);

            _database.Add(activity);
            await _database.SaveChangesAsync();

            reservations.ForEach(r => r.ActivityId = activity.Id);

            await _database.Reservations.AddRangeAsync(reservations);

            activity.Organizators = new List<UserActivityOrganizator>
            {
                new UserActivityOrganizator { ActivityId = activity.Id, UserId = user.Id }
            };

            await _database.SaveChangesAsync();
        }


        public async Task AddVisitorAsync(Activity activity, string userEmail)
        { ;
            var user = await _database.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            activity.Visitors.Add(new UserActivityVisitor { UserId = user.Id, ActivityId = activity.Id });
            _database.SaveChanges();
        }

        public async Task DeleteVisitorAsync(Activity activity, string userEmail)
        { ;
            var user = await _database.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            var visitor = activity.Visitors.FirstOrDefault(v => v.UserId == user.Id);
            activity.Visitors.Remove(visitor);

            _database.SaveChanges();
        }

        public async Task DeleteAsync(Activity activity)
        {
            _database.Activities.Remove(activity);
            await _database.SaveChangesAsync();
        }

        public async Task<Activity> GetActivityAsync(string name)
        {
            var activityModel = await _database.Activities
                .Include(a => a.Reservations)
                .ThenInclude(a => a.Room)
                .Include(a => a.Visitors)
                .FirstOrDefaultAsync(r => r.Name == name);
            return activityModel;
        }

        public async Task<Activity> GetActivityAsync(Guid id)
        {
            var activityModel = await _database.Activities
                .Include(a => a.Reservations)
                .ThenInclude(a => a.Room)
                .Include(a => a.Visitors)
                .ThenInclude(a => a.User)
                .Include(a => a.Organizators)
                .ThenInclude(a => a.User)
                .FirstOrDefaultAsync(r => id == r.Id);
            return activityModel;
        }

        public async Task<IList<Activity>> GetAllActivitiesAsync()
        {
            var activityModels = await _database.Activities
                .Include(a => a.Reservations)
                .ThenInclude(a => a.Room)
                .Include(a => a.Visitors)
                .Include(a => a.Organizators)
                .ThenInclude(a => a.User)
                .ToListAsync();
            return activityModels;
        }

        public async Task UpdateAsync(Activity activity)
        {
            var activityToUpdate = await _database.Activities.FirstOrDefaultAsync(r => r.Id == activity.Id);

            activityToUpdate.Name = activity.Name;
            activityToUpdate.PeriodType = activity.PeriodType;
            activityToUpdate.StartDate = activity.StartDate;
            activityToUpdate.Description = activity.Description;
            activityToUpdate.EndDate = activity.EndDate;

            await _database.SaveChangesAsync();
        }

        public async Task<bool> HasPermissionToDeleteEvent(Activity activity, string userEmail)
        {
            var user = await _database.Users
                .Include(u => u.UserRoles)
                .ThenInclude(r => r.Role)
                .FirstOrDefaultAsync(u => u.Email == userEmail);

            bool isAdmin = user.UserRoles.Select(ur => ur.Role).Any(r => r.Name.ToLowerInvariant() == "admin".ToLowerInvariant());
            return isAdmin || activity.Organizators.Any(o => o.UserId == user.Id);
        }
    }
}
