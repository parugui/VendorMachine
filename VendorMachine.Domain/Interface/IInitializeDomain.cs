using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace VendorMachine.Domain.Interface
{
	public interface IInitializeDomain
	{
		void Initialize(ServiceProvider _provider);
	}
}
