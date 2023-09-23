using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ManageMySpace.Common.EF;
using ManageMySpace.Common.EF.Models;
using ManageMySpace.UserService.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ManageMySpace.UserService.DAL
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ManageMySpaceContext _database;

        public RoleRepository(ManageMySpaceContext database)
        {
            _database = database;
        }

        public async Task AddAsync(Role role)
        {
            await _database.Roles.AddAsync(role);
            await _database.SaveChangesAsync();
        }

        public async Task<Role> GetAsync(string name)
        {
            var role = (await _database.Roles.ToListAsync()).FirstOrDefault(r => r.Name.ToLowerInvariant() == name.ToLowerInvariant());
            return role;
        }

        public async Task<IEnumerable<Role>> Browse()
        {
            var roles = await _database.Roles.ToListAsync();
            return roles;
        }

        public async Task RemoveAsync(string roleName)
        {
            var role = await _database.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            _database.Roles.Remove(role);
            await _database.SaveChangesAsync();
        }
    }
}
