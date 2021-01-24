using System;
using System.Collections.Generic;
using System.Text;
using VendorMachine.Domain.Interface;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using VendorMachine.Domain.Models;

namespace VendorMachine.Domain.Test
{
	public class InitializeMachineDomainTest: DomainTestBase
	{

		[Fact]
		public void InitializeMachineTest()
		{
			DomainBase.provider.GetService<IInitializeMachineDomain>().Execute();
			Assert.Equal(60, MachineDomain.AvailableCoins.Count);
			Assert.Equal(3, MachineDomain.AvailableProducts.Count);
		}

		
	}
}
