using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManageMySpace.Common.EF.Models;
using ManageMySpace.UserService.BLL.Interfaces;
using ManageMySpace.UserService.DAL.Interfaces;

namespace ManageMySpace.UserService.BLL.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IList<Role>> GetAllAsync()
        {
            return (await _roleRepository.Browse()).ToList();
        }

        public async Task AddAsync(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException(nameof(roleName));
            }

            var role = new Role();
            role.Name = roleName;

            await _roleRepository.AddAsync(role);
        }

        public async Task RemoveAsync(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                throw  new ArgumentNullException(nameof(roleName));
            }

            await _roleRepository.RemoveAsync(roleName);
        }
    }
}
