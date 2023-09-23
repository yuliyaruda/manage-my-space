using System;
using System.Collections.Generic;

namespace ManageMySpace.Common.EF.Models
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
