using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using VendorMachine.Domain.Interface;
using VendorMachine.Domain.Services;

namespace VendorMachine.Domain
{
	public static class IServiceCollectionExtension
	{
		public static IServiceCollection AddDomainServiceCollection(this IServiceCollection services)
		{
			services.AddSingleton<IInitializeDomain, InitializeDomain>();

			services.AddScoped<IInitializeMachineDomain, InitializeMachineDomain>();
			services.AddScoped<IInsertCoinsDomain, InsertCoinsDomain>();
			services.AddScoped<IGetChangeDomain, GetChangeDomain>();
			services.AddScoped<IRequestProductDomain, RequestProductDomain>();
			return services;
		}
	}
}
