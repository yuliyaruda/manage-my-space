using AutoMapper;
using ManageMySpace.ActivityService.BLL;
using ManageMySpace.ActivityService.BLL.Interfaces;
using ManageMySpace.ActivityService.DAL;
using ManageMySpace.ActivityService.DAL.Interfaces;
using ManageMySpace.Common.Auth;
using ManageMySpace.Common.EF;
using ManageMySpace.Common.RabbitMQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ManageMySpace.ActivityService
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
            services.AddAutoMapper(typeof(Startup).Assembly);
            services.AddControllers().AddNewtonsoftJson();
            services.AddJwt(Configuration);
            services.AddRabbitMq(Configuration);
            services.AddDbContext<ManageMySpaceContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:ManageMySpace.UserService"]), ServiceLifetime.Transient, ServiceLifetime.Singleton);
            services.AddTransient<IActivityRepository, ActivityRepository>();
            services.AddTransient<IRoomRepository, RoomRepository>();
            services.AddTransient<IActivityService, BLL.ActivityService>();
            services.AddTransient<IRoomService, RoomService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
