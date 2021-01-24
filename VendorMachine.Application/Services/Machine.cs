using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VendorMachine.Application.Interfaces;
using VendorMachine.Application.ViewModels;
using VendorMachine.Domain.Interface;
using VendorMachine.Domain.Models;
using VendorMachine.Utils;

namespace VendorMachine.Application.Services
{
	public class Machine : ApplicationBase, IMachine
	{
		public void Initialize()
		{
			var domain = GetService<IInitializeMachineDomain>();
			domain.Execute();
			
		}

		public void InsertCoins(List<vmCoin> ListCoins)
		{
			var domain = GetService<IInsertCoinsDomain>();
			List<CoinDomain> coins = DataHelper.List<CoinDomain>(ListCoins);
			domain.Execute(coins);

		}

		public vmMachine RequestProduct(List<vmProduct> ListProduct)
		{
			var domain = GetService<IRequestProductDomain>();
			List<ProductDomain> products = DataHelper.List<ProductDomain>(ListProduct);
			domain.Execute(products);
			return ConvertMachineToViewModel();
		}

		public vmMachine GetChange()
		{
			var domain = GetService<IGetChangeDomain>();
			domain.Execute();
			return ConvertMachineToViewModel();
		}


		private vmMachine ConvertMachineToViewModel()
		{
			vmMachine VendorMachine = new vmMachine();
			VendorMachine.AvailableCoins = DataHelper.List<vmCoin>(MachineDomain.AvailableCoins);
			VendorMachine.AvailableProducts = DataHelper.List<vmProduct>(MachineDomain.AvailableProducts);
			VendorMachine.DueChange = DataHelper.List<vmCoin>(MachineDomain.DueChange);
			VendorMachine.InsertedCoins = DataHelper.List<vmCoin>(MachineDomain.InsertedCoins);
			VendorMachine.RequiredProducts = DataHelper.List<vmProduct>(MachineDomain.RequiredProducts);
			VendorMachine.OutputProducts = DataHelper.List<vmProductOutput>(MachineDomain.OutputProducts);

			VendorMachine.TotalInsertedCoins = MachineDomain.InsertedCoins.Sum(c => c.Coin);
			VendorMachine.TotalRequiredProducts = TotalRequiredProducts();

			return VendorMachine;
		}

		private double TotalRequiredProducts()
		{
			double priceProduct = 0;
			foreach (ProductDomain product in MachineDomain.RequiredProducts)
			{
				var AvailableProduct = MachineDomain.AvailableProducts.FirstOrDefault(p => p.Name.ToUpper() == product.Name.ToUpper());
				priceProduct += AvailableProduct.Price * product.Quantity;
			}

			return priceProduct;

		}

		
	}
}
