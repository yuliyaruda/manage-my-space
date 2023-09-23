using System;
using System.Collections.Generic;

namespace ManageMySpace.Common.Events.ActivityEvents
{
    public class ActivityCanceled : IEvent
    {
        public List<string> VisitorUserEmails { get; set; }
        public string ActivityName { get; set; }
        public DateTime ActivityDate { get; set; }

        public ActivityCanceled() { }

        public ActivityCanceled(List<string> visitorUserEmails, string activityName, DateTime activityDate)
        {
            VisitorUserEmails = visitorUserEmails;
            ActivityName = activityName;
            ActivityDate = activityDate;
        }
    }
}
