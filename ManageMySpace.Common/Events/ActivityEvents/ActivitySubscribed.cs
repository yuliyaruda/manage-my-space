using System;

namespace ManageMySpace.Common.Events.ActivityEvents
{
    public class ActivitySubscribed : IEvent
    {
        public string UserEmail { get; set; }
        public string ActivityName { get; set; }

        protected ActivitySubscribed() { }

        public ActivitySubscribed(string userEmail, string activityName)
        {
            UserEmail = userEmail;
            ActivityName = activityName;
        }
    }
}
