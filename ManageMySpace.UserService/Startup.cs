using ManageMySpace.Common.Auth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using ManageMySpace.Common.Commands;
using ManageMySpace.Common.Commands.UserCommands;
using ManageMySpace.Common.RabbitMQ;
using ManageMySpace.Common.EF;
using AutoMapper;
using ManageMySpace.UserService.BLL.Interfaces;
using ManageMySpace.UserService.BLL.Services;
using ManageMySpace.UserService.DAL;
using ManageMySpace.UserService.DAL.Interfaces;

namespace ManageMySpace.UserService
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
            services.AddControllers();
            services.AddAutoMapper(typeof(Startup));
            services.AddJwt(Configuration);
            services.AddRabbitMq(Configuration);
            services.AddDbContext<ManageMySpaceContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:ManageMySpace.UserService"]), ServiceLifetime.Transient, ServiceLifetime.Singleton);
            services.AddSingleton<IEncrypter, Encrypter>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IUserService, BLL.Services.UserService>();
            services.AddTransient<IRoleService, RoleService>();
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
