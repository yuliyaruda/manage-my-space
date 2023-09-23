using System;

namespace ManageMySpace.Common.Commands.UserCommands
{
    public class BanUser: IAuthCommand
    {
        public Guid InvokerId { get; set; }
        public string UserEmail { get; set; }
    }
}
