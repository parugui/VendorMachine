using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using VendorMachine.Domain;
using VendorMachine.Domain.Interface;

namespace VendorMachine.Application.Services
{
	public class ApplicationBase
	{
		public ServiceProvider provider { get; private set; }

		public ApplicationBase()
		{
			var service = new ServiceCollection();
			service.AddDomainServiceCollection();

			provider = service.BuildServiceProvider();
		}

		public T GetService<T>()
		{
			provider.GetService<IInitializeDomain>().Initialize(provider);
			return provider.GetService<T>();
		}
	}
}
