using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ManageMySpace.Common.EF.Models;

namespace ManageMySpace.UserService.DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetAsync(string email);
        Task<User> GetAsync(Guid id);
        Task<IList<User>> GetAllAsync();
        Task AddAsync(User user);
        Task SaveChanges();
    }
}
