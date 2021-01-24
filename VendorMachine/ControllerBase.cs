using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VendorMachine.Application;

namespace VendorMachine
{
	public class ControllerBase
	{
		public ServiceProvider provider { get; private set; }
        public IConfiguration configuration { get; private set; }

        public ControllerBase()
        {
            var service = new ServiceCollection();
            service.AddApplicationServiceCollection();

            provider = service.BuildServiceProvider();
            ConfigurationBase();
        }

        public void ConfigurationBase()
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            configuration = builder.Build();
        }
    }
}
