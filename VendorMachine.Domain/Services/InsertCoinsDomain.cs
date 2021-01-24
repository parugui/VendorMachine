using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using VendorMachine.Domain.Interface;
using VendorMachine.Domain.Models;

namespace VendorMachine.Domain.Services
{
	public class InsertCoinsDomain : DomainBase, IInsertCoinsDomain
	{
		public void Execute(List<CoinDomain> coins)
		{
			CultureInfo formatProvider = new CultureInfo("us-EN", false);
			string AllowedCoins = config.GetSection("AllowedCoins").Value;
			foreach (CoinDomain coin in coins)
			{
				string[] allowedCoin = AllowedCoins.Split(",");

				var c = allowedCoin.FirstOrDefault(s => s == coin.Coin.ToString("0.00", formatProvider));
				if (c == null)
				{
					throw new ValidationException(String.Format("Coin not Accepted: {0}. Please Insert only {1}", coin.Coin.ToString("0.00", formatProvider), AllowedCoins.Replace(",", ", ")));
				}

				MachineDomain.AvailableCoins.Add(coin);
			}

			MachineDomain.InsertedCoins = coins;
		}
	}
}
