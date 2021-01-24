using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace VendorMachine.Domain
{
	public class DomainBase
	{
		public static IConfiguration config;
		public static ServiceProvider provider { get; set; }

		public DomainBase()
		{
			RepositoryBase();
		}

		public static void RepositoryBase()
		{
			var builder = new ConfigurationBuilder()
			  .SetBasePath(Directory.GetCurrentDirectory())
			  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

			config = builder.Build();
		}

		public static string GetAvailableProduct()
		{
			string AvailableProducts = config.GetSection("AvailableProduct").Value;
			StringBuilder Products = new StringBuilder();

			foreach (string configProducts in AvailableProducts.Split(";"))
			{
				Products.AppendFormat("{0}, ", configProducts.Split(",")[1]);
			}

			return Products.ToString();

		}
	}
}
