using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using VendorMachine.Domain.Interface;

namespace VendorMachine.Domain
{
	public class InitializeDomain : IInitializeDomain
	{
		public void Initialize(ServiceProvider _provider)
		{
			if (DomainBase.provider == null)
				DomainBase.provider = _provider;
		}
	}
}
