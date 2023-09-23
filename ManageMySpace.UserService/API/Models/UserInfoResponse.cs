using System;

namespace ManageMySpace.UserService.API.Models
{
    public class UserInfoResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Banned { get; set; }
    }
}
