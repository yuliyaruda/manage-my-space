namespace ManageMySpace.Common.Events.ActivityEvents
{
    public class ActivityCreated : IEvent
    {
        public string UserEmail { get; set; }
        public string ActivityName { get; set; }

        public ActivityCreated() { }

        public ActivityCreated(string userEmail, string activityName)
        {
            UserEmail = userEmail;
            ActivityName = activityName;
        }
    }
}
