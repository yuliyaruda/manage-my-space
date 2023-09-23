using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ManageMySpace.Common.Auth;
using ManageMySpace.Common.EF.Models;

namespace ManageMySpace.UserService.BLL.Interfaces
{
    public interface IUserService
    {
        Task RegisterAsync(string email, string name, string password, string lastName);
        Task<JsonWebTocken> LoginAsync(string email, string password);
        Task BanUserAsync(Guid invokerId, string email);
        Task UnblockUserAsync(Guid invokerId, string email);
        Task AssignRoleToUser(string invokerUserEmail, string userRole, string userName);
        Task<User> GetAsync(string email);
        Task<IList<User>> GetAllAsync();
    }
}
