namespace ManageMySpace.Common.Events.UserEvents
{
    public class UserCreated : IEvent
    {
        public string Email { get; set; }
        public string Name { get; set; }

        protected UserCreated() { }

        public UserCreated( string email, string name)
        {
            Email = email;
            Name = name;
        }
    }
}
