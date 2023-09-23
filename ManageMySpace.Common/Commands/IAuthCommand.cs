using System;

namespace ManageMySpace.Common.Commands
{
    public interface IAuthCommand : ICommand
    {
        Guid InvokerId { get; set; }
    }
}
