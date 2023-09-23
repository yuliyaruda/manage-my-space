using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ManageMySpace.Common.EF;
using ManageMySpace.Common.EF.Models;
using ManageMySpace.UserService.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ManageMySpace.UserService.DAL
{
    public class UserRepository : IUserRepository
    {
        private readonly ManageMySpaceContext _database;

        public UserRepository(ManageMySpaceContext database)
        {
            _database = database;
        }
        
        public async Task AddAsync(User user)
        {
            await _database.Users.AddAsync(user);
            await _database.SaveChangesAsync();
        }

        public async Task SaveChanges()
        {
            await _database.SaveChangesAsync();
        }

        public async Task<User> GetAsync(string email)
        {
            var userModel = await _database.Users
                .Include(u => u.UserRoles)
                .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(r => r.Email == email);

            return userModel;
        }

        public async Task<User> GetAsync(Guid id)
        {
            var userModel = await _database.Users
                .Include(u => u.UserRoles)
                .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(r => r.Id == id);

            return userModel;
        }

        public async Task UpdateUserRolesAsync(User user, Role role)
        {
            var userModel = await _database.Users
                .Include(u => u.UserRoles)
                .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(r => r.Email == user.Email);

            userModel.UserRoles = new List<UserRole>();
            userModel.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = role.Id });
            await _database.SaveChangesAsync();
        }

        public async Task<IList<User>> GetAllAsync()
        {
            var userModels = await _database.Users
                .Include(u => u.UserRoles)
                .ThenInclude(u => u.Role)
                .ToListAsync();
            return userModels;
        }
    }
}
