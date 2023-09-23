using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManageMySpace.Common.Auth;
using ManageMySpace.Common.EF.Models;
using ManageMySpace.Common.Enums;
using ManageMySpace.Common.Exceptions;
using ManageMySpace.UserService.BLL.Interfaces;
using ManageMySpace.UserService.DAL.Interfaces;
using UserRole = ManageMySpace.Common.EF.Models.UserRole;

namespace ManageMySpace.UserService.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IEncrypter _encrypter;
        private readonly IJwtHandler _jwtHandler;

        public UserService(IUserRepository userRepository, IEncrypter encrypter, IJwtHandler jwtHandler, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _encrypter = encrypter;
            _jwtHandler = jwtHandler;
            _roleRepository = roleRepository;
        }

        public async Task BanUserAsync(Guid invokerId, string email)
        {           
            if (await IsUserAdmin(invokerId))
            {
                var userToBan = await _userRepository.GetAsync(email);
                userToBan.Banned = true;
                await _userRepository.SaveChanges();
            }
            else
            {
                throw new ManageMySpaceException("access_denied",
                    $"Access denied. You do not have permissions to ban user with email: {email}.");
            }
        }

        public async Task<JsonWebTocken> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetAsync(email);

            if(user != null && !user.Banned && user.Password.Equals(_encrypter.GetHash(password, user.Salt)))
            {
                return _jwtHandler.Create(user.Id, user.UserRoles.FirstOrDefault()?.Role.Name, user.Email);
            }
            throw new ManageMySpaceException("invalid_password",
                    $"Invalid password for user {email}.");
        }

        public async Task RegisterAsync(string email, string name, string password, string lastName)
        {
            bool isAnyUsers = (await _userRepository.GetAllAsync()).Any();
            var roleName = isAnyUsers ? Common.Enums.UserRole.Visitor.ToString() : Common.Enums.UserRole.Admin.ToString();
            var role = await _roleRepository.GetAsync(roleName);
            var user = new User
            {
                Email = email,
                Name = name,
                LastName = lastName
            };

            if (!(await _userRepository.GetAsync(email) is null))
            {
                throw new ManageMySpaceException("emailIsUsed", "Email is in used.");
            }

            user.Salt = _encrypter.GetSalt(password);
            user.Password = _encrypter.GetHash(password, user.Salt);

            user.UserRoles = new List<UserRole> {new UserRole {RoleId = role.Id, User = user}};

            await _userRepository.AddAsync(user);
        }

        public async Task AssignRoleToUser(string invokerUserEmail, string userRole, string userEmail)
        {
            if (!await IsUserAdmin(invokerUserEmail))
            {
                throw new ManageMySpaceException("access_denied",
                    $"Access denied. You do not have permissions to update user's role: {userEmail}.");
            }

            var role = await _roleRepository.GetAsync(userRole);
            var user = await _userRepository.GetAsync(userEmail);

            if(user != null && role != null)
            {
                user.UserRoles.Clear();
                user.UserRoles.Add(new UserRole {UserId = user.Id, RoleId = role.Id});
                await _userRepository.SaveChanges();
            }
        }

        private async Task<bool> IsUserAdmin(Guid userId)
        {
            var updateByUser = await _userRepository.GetAsync(userId);
            return updateByUser.UserRoles.Select(ur => ur.Role).Any(r => r.Name.ToLowerInvariant() == Common.Enums.UserRole.Admin.ToString().ToLowerInvariant());
        }

        private async Task<bool> IsUserAdmin(string userEmail)
        {
            var updateByUser = await _userRepository.GetAsync(userEmail);
            return updateByUser.UserRoles.Select(ur => ur.Role).Any(r => r.Name.ToLowerInvariant() == Common.Enums.UserRole.Admin.ToString().ToLowerInvariant());
        }

        public async Task UnblockUserAsync(Guid invokerId, string email)
        {
            if (await IsUserAdmin(invokerId))
            {
                var userToUnblock = await _userRepository.GetAsync(email);
                userToUnblock.Banned = false;
                await _userRepository.SaveChanges();
            }
            else
            {
                throw new ManageMySpaceException("access_denied",
                    $"Access denied. You do not have permissions to ban user with email: {email}.");
            }
        }

        public async Task<User> GetAsync(string email)
        {
            return await _userRepository.GetAsync(email);
        }

        public Task<IList<User>> GetAllAsync()
        {
            return _userRepository.GetAllAsync();
        }
    }
}
