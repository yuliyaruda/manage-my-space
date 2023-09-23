using System;

namespace ManageMySpace.Common.Commands.UserCommands
{
    public class AssignRoleToUser : IAuthCommand
    {
        public Guid InvokerId { get; set; }
        public string UserEmail { get; set; }
        public string RoleName { get; set; }
    }
}
