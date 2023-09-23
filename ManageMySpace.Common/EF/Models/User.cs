using System;
using System.Collections.Generic;

namespace ManageMySpace.Common.EF.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Salt { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<UserActivityOrganizator> OrganizedEvents { get; set; }
        public ICollection<UserActivityVisitor> VisitedEvents { get; set; }
        public bool Banned { get; set; }
    }
}
