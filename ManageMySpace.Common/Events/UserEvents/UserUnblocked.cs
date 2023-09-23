using System;

namespace ManageMySpace.Common.Events.UserEvents
{
    public class UserUnblocked : IAuthEvent
    {
        public Guid UserId { get; set; }
        public string BannedUserEmail { get; set; }
        public string BannedUserName { get; set; }

        protected UserUnblocked() { }
        public UserUnblocked(Guid userId, string bannedUserEmail, string bannedUserName)
        {
            UserId = userId;
            BannedUserEmail = bannedUserEmail;
            BannedUserName = bannedUserName;
        }
    }
}
