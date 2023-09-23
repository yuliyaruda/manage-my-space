using System.Collections.Generic;
using System.Threading.Tasks;
using ManageMySpace.Common.EF.Models;

namespace ManageMySpace.UserService.DAL.Interfaces
{
    public interface IRoleRepository
    {
        Task AddAsync(Role role);
        Task<Role> GetAsync(string name);
        Task RemoveAsync(string roleName);
        Task<IEnumerable<Role>> Browse();
    }
}
