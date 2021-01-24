using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using VendorMachine.Domain.Interface;
using VendorMachine.Domain.Models;

namespace VendorMachine.Domain.Services
{
	public class GetChangeDomain : IGetChangeDomain
	{
		

		public void Execute()
		{
			double priceProduct = 0;
			double InsertedCoins = MachineDomain.InsertedCoins.Sum(c => c.Coin);
			MachineDomain.DueChange = new List<CoinDomain>();

			foreach (ProductDomain product in MachineDomain.RequiredProducts)
			{
				var AvailableProduct = MachineDomain.AvailableProducts.FirstOrDefault(p => p.Name.ToUpper() == product.Name.ToUpper());
				priceProduct += Math.Round(AvailableProduct.Price * product.Quantity, 2);
			}

			double DueChange = Math.Round(InsertedCoins - priceProduct, 2);

			while (DueChange > 0)
			{

				CoinDomain coin = MachineDomain.AvailableCoins.OrderByDescending(c => c.Coin).FirstOrDefault(c => c.Coin <= DueChange);

				if (coin == null)
				{
					throw new ValidationException("NO_COINS. Sorry, I owe you..");
				}

				MachineDomain.DueChange.Add(coin);
				MachineDomain.AvailableCoins.Remove(coin);
				DueChange = Math.Round(DueChange - coin.Coin, 2);
			}

		}
	}
}
