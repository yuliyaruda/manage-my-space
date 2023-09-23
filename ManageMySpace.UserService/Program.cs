using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManageMySpace.Common.Commands.UserCommands;
using ManageMySpace.Common.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ManageMySpace.UserService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ServiceHost.Create<Startup>(args)
                .UseRabbitMq()
                .Build()
                .Run();
        }
    }
}
