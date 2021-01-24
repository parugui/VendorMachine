using System;
using System.Collections.Generic;
using System.Text;
using VendorMachine.Application.ViewModels;
using VendorMachine.Utils;
using System.Linq;
using System.Globalization;

namespace VendorMachine
{
	class Arguments
	{

		public List<vmCoin> ListCoins { get; set; }
		public List<vmProduct> ListProducts { get; set; }

		public Arguments(string Sentence)
		{
			ListCoins = new List<vmCoin>();
			ListProducts = new List<vmProduct>();
			foreach (string param in Sentence.Split(" "))
			{
				if (Validation.isDouble(param))
				{
					ListCoins.Add(new vmCoin { Coin = Convert.ToDouble(param, new CultureInfo("us-EN", false)) });
				}
				else if (!string.IsNullOrEmpty(param) && param.ToUpper() != "CHANGE" )
				{
					var product = ListProducts.FirstOrDefault(p => p.Name.ToUpper() == param.ToUpper());
					if (product == null)
					{
						ListProducts.Add(new vmProduct { Name = param, Quantity = 1 });
					}
					else
					{
						product.Quantity++;
					}
				}

			}

		}
	}
}
