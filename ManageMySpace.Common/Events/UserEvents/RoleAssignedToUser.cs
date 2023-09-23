using System;
using System.Collections.Generic;
using System.Text;

namespace ManageMySpace.Common.Events.UserEvents
{
    public class RoleAssignedToUser : IAuthEvent
    {
        public Guid UserId { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }

        protected RoleAssignedToUser() { }
        public RoleAssignedToUser(Guid userId, string userEmail, string roleName, string userName)
        {
            UserId = userId;
            UserEmail = userEmail;
            RoleName = roleName;
            UserName = userName;
        }
    }
}
