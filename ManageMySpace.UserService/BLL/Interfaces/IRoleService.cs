using System.Collections.Generic;
using System.Threading.Tasks;
using ManageMySpace.Common.EF.Models;

namespace ManageMySpace.UserService.BLL.Interfaces
{
    public interface IRoleService
    {
        Task<IList<Role>> GetAllAsync();
        Task AddAsync(string roleName);
        Task RemoveAsync(string roleName);
    }
}
