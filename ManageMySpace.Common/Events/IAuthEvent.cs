using System;

namespace ManageMySpace.Common.Events
{
    public interface IAuthEvent : IEvent
    {
        Guid UserId { get; set; }
    }
}
