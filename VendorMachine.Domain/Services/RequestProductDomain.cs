using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VendorMachine.Domain.Interface;
using VendorMachine.Domain.Models;
using System.Linq;

namespace VendorMachine.Domain.Services
{
	public class RequestProductDomain : DomainBase, IRequestProductDomain
	{
		public void Execute(List<ProductDomain> ListProducts)
		{

			if (ExecuteValidation(ListProducts))
			{
				MachineDomain.RequiredProducts = ListProducts;
				MachineDomain.DueChange = new List<CoinDomain>();
				MachineDomain.OutputProducts = new List<ProductOutputDomain>();
				OrderSnack();
			}

		}

		private void OrderSnack()
		{
			double InsertedCoins = MachineDomain.InsertedCoins.Sum(c => c.Coin);
			MachineDomain.OutputProducts = new List<ProductOutputDomain>();

			foreach (ProductDomain product in MachineDomain.RequiredProducts)
			{
				ProductDomain AvailableProduct = MachineDomain.AvailableProducts.FirstOrDefault(c => c.Name.ToUpper() == product.Name.ToUpper());
				AvailableProduct.Quantity -= product.Quantity;
				for (int i = 0; i < product.Quantity; i++)
				{
					InsertedCoins -= AvailableProduct.Price;

					ProductOutputDomain output = new ProductOutputDomain { Name = AvailableProduct.Name };
					output.Change = Math.Round(InsertedCoins, 2);
					MachineDomain.OutputProducts.Add(output);
				}
			}		
		}

		private bool ExecuteValidation(List<ProductDomain> ListProducts)
		{

			if (MachineDomain.InsertedCoins == null || MachineDomain.InsertedCoins.Count == 0)
			{
				throw new ValidationException("Please Insert some Coins..");
			}
			else if (!isValidProduct(ListProducts))
			{
				throw new ValidationException(String.Format("NO_PRODUCT. Please request only {0}", DomainBase.GetAvailableProduct()));
			}
			else if (!isAvailableQttProduct(ListProducts))
			{
				throw new ValidationException("Unavailable Product. Insufficient Quantity. Sorry!");
			}
			else if (!isEnoughMoney(ListProducts))
			{
				throw new ValidationException("Insufficiente Coins to order the amount of product. Please Insert more Coins.");
			}

			return true;
		}

		private bool isValidProduct(List<ProductDomain> ListProducts)
		{
			string AvailableProducts = DomainBase.GetAvailableProduct();
			foreach (ProductDomain product in ListProducts)
			{
				if (!AvailableProducts.ToUpper().Contains(product.Name.ToUpper()))
				{
					return false;
				}
			}
			return true;
		}

		private bool isAvailableQttProduct(List<ProductDomain> ListProducts)
		{
			
			foreach (ProductDomain product in ListProducts) 
			{
				if (product.Quantity > MachineDomain.AvailableProducts.FirstOrDefault(c => c.Name.ToUpper() == product.Name.ToUpper()).Quantity)
				{
					return false;
				}
			}

			return true;
		}

		private bool isEnoughMoney(List<ProductDomain> ListProducts)
		{
			double priceProduct = 0;
			double InsertedCoins = MachineDomain.InsertedCoins.Sum(c => c.Coin);
			foreach (ProductDomain product in ListProducts)
			{
				var AvailableProduct = MachineDomain.AvailableProducts.FirstOrDefault(p => p.Name.ToUpper() == product.Name.ToUpper());
				priceProduct += AvailableProduct.Price * product.Quantity;
			}

			if (InsertedCoins < priceProduct)
			{
				return false;
			}

			return true;
		}
	}
}
