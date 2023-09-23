using ManageMySpace.Common.Events.ActivityEvents;
using ManageMySpace.Common.Events.UserEvents;
using ManageMySpace.Common.Services;

namespace ManageMySpace.MailNotificationService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ServiceHost.Create<Startup>(args)
                .UseRabbitMq()
                .SubscribeToEvent<UserCreated>()
                .SubscribeToEvent<RoleAssignedToUser>()
                .SubscribeToEvent<UserBanned>()
                .SubscribeToEvent<UserUnblocked>()
                .SubscribeToEvent<ActivityCanceled>()
                .SubscribeToEvent<ActivityCreated>()
                .SubscribeToEvent<ActivitySubscribed>()
                .Build()
                .Run();
        }
    }
}
