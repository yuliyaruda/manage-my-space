namespace ManageMySpace.Common.Events.UserEvents
{
    public class UserAuthenticated : IEvent
    {
        public string Email { get; set; }

        protected UserAuthenticated() { }

        public UserAuthenticated(string email)
        {
            Email = email;
        }
    }
}
