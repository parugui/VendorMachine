using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using VendorMachine.Domain.Interface;
using VendorMachine.Domain.Models;

namespace VendorMachine.Domain.Services
{
	public class InitializeMachineDomain : DomainBase, IInitializeMachineDomain
	{
		public void Execute()
		{
			MachineDomain.AvailableCoins = new List<CoinDomain>();
			MachineDomain.AvailableProducts = new List<ProductDomain>();
			MachineDomain.DueChange = new List<CoinDomain>();
			MachineDomain.InsertedCoins = null;
			MachineDomain.RequiredProducts = null;
			InsertAvailableCoins();
			InsertAvailableProducts();
		}

		private void InsertAvailableCoins()
		{
			string coins = config.GetSection("AllowedCoins").Value;
			foreach (string coin in coins.Split(","))
			{
				InsertCoins(Convert.ToDouble(coin, new CultureInfo("us-EN", false)));
			}

		}

		private void InsertCoins(double value)
		{
			List<CoinDomain> ListCoins = new List<CoinDomain>();

			for (int i = 0; i < 10; i++)
			{
				ListCoins.Add(new CoinDomain { Coin = value });
			}
			 
			MachineDomain.AvailableCoins.AddRange(ListCoins);
		}

		private void InsertAvailableProducts()
		{
			string AvailableProducts = config.GetSection("AvailableProduct").Value;
			foreach (string product in AvailableProducts.Split(";"))
			{
				ProductDomain prod = new ProductDomain();
				prod.ProductId = Convert.ToInt32(product.Split(",")[0]);
				prod.Name = product.Split(",")[1];
				prod.Price = Convert.ToDouble(product.Split(",")[2], new CultureInfo("us-EN", false));

				InsertProduct(prod);
			}

		}

		private void InsertProduct(ProductDomain product)
		{
			List<ProductDomain> ListProducts = new List<ProductDomain>();
			product.Quantity = 10;
			ListProducts.Add(product);
			
			MachineDomain.AvailableProducts.AddRange(ListProducts);
		}

	}
}
