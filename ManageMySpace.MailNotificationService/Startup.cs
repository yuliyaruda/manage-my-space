using ManageMySpace.Common.Events;
using ManageMySpace.Common.Events.ActivityEvents;
using ManageMySpace.Common.Events.UserEvents;
using ManageMySpace.Common.RabbitMQ;
using ManageMySpace.MailNotificationService.Core;
using ManageMySpace.MailNotificationService.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ManageMySpace.MailNotificationService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var options = new MessageSenderSettings();
            var section = Configuration.GetSection(nameof(MessageSenderSettings));
            section.Bind(options);
            services.AddSingleton(options);

            services.AddControllers();
            services.AddRabbitMq(Configuration);
            services.AddSingleton<IMessageSender, MessageSender>();
            
            services.AddSingleton<IEventHandler<UserCreated>, UserCreatedHandler>();
            services.AddSingleton<IEventHandler<RoleAssignedToUser>, RoleAssignedToUserHandler>();
            services.AddSingleton<IEventHandler<UserBanned>, UserBannedHandler>();
            services.AddSingleton<IEventHandler<UserUnblocked>, UserUnblockedHandler>();

            services.AddSingleton<IEventHandler<ActivityCanceled>, ActivityCanceledHandler>();
            services.AddSingleton<IEventHandler<ActivityCreated>, ActivityCreatedHandler>();
            services.AddSingleton<IEventHandler<ActivitySubscribed>, ActivitySubscribedHandler>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
