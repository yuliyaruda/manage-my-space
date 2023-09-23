using System;

namespace ManageMySpace.Common.Events.UserEvents.Rejected
{
    public class BanUserRejected : IRejectedEvent
    {
        public string Reason { get; protected set; }
        public string Code { get; protected set; }
        public string UserEmailToBan { get; protected set; }
        public Guid UserTryToBan { get; protected set; }

        protected BanUserRejected() { }

        public BanUserRejected(string userEmailToBan, Guid userTryToBan, string reason, string code)
        {
            Reason = reason;
            Code = code;
            UserEmailToBan = userEmailToBan;
            UserTryToBan = userTryToBan;
        }
    }
}
