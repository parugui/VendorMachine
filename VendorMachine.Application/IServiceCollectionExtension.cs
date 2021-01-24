using Microsoft.Extensions.DependencyInjection;
using System;
using VendorMachine.Application.Interfaces;
using VendorMachine.Application.Services;

namespace VendorMachine.Application
{
	public static class IServiceCollectionExtension
	{

		public static IServiceCollection AddApplicationServiceCollection(this IServiceCollection services)
		{
			services.AddScoped<IMachine, Machine>();
			return services;
		}

	}
}
