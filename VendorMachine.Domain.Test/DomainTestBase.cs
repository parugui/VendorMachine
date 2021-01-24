using Microsoft.Extensions.DependencyInjection;
using System;
using VendorMachine.Domain.Interface;
using Xunit;

namespace VendorMachine.Domain.Test
{
	public class DomainTestBase
	{

		public ServiceProvider provider { get; private set; }
        public DomainTestBase()
        {
            var service = new ServiceCollection();
            service.AddDomainServiceCollection();
            //service.AddInfraTestServiceCollection(); //MOCK

            provider = service.BuildServiceProvider();

            provider.GetService<IInitializeDomain>().Initialize(provider);
        }
    }
}
