using System;
using System.Collections.Generic;
using System.Text;
using VendorMachine.Domain.Interface;
using VendorMachine.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace VendorMachine.Domain.Test
{
	public class GetChangeDomainTest: DomainTestBase
	{
		[Fact]
		public void GetChangeSucessTest()
		{
			List<CoinDomain> ListCoins = new List<CoinDomain>();
			ListCoins.Add(new CoinDomain { Coin = 1.00 });

			List<ProductDomain> ListProduct = new List<ProductDomain>();
			ListProduct.Add(new ProductDomain { Name = "Pastelina", Quantity = 3 });

			DomainBase.provider.GetService<IInitializeMachineDomain>().Execute();
			DomainBase.provider.GetService<IInsertCoinsDomain>().Execute(ListCoins);
			DomainBase.provider.GetService<IRequestProductDomain>().Execute(ListProduct);

			DomainBase.provider.GetService<IGetChangeDomain>().Execute();
			Assert.Single(MachineDomain.DueChange);
			Assert.Equal(0.10, MachineDomain.DueChange[0].Coin);
			Assert.Equal(9, MachineDomain.AvailableCoins.FindAll(c => c.Coin == 0.10).Count);

		}

		[Fact]
		public void GetChangeSucessPastelina1Test()
		{
			List<CoinDomain> ListCoins = new List<CoinDomain>();
			ListCoins.Add(new CoinDomain { Coin = 1.00 });

			List<ProductDomain> ListProduct = new List<ProductDomain>();
			ListProduct.Add(new ProductDomain { Name = "Pastelina", Quantity = 1 });

			DomainBase.provider.GetService<IInitializeMachineDomain>().Execute();
			DomainBase.provider.GetService<IInsertCoinsDomain>().Execute(ListCoins);
			DomainBase.provider.GetService<IRequestProductDomain>().Execute(ListProduct);

			DomainBase.provider.GetService<IGetChangeDomain>().Execute();
			Assert.Equal(3, MachineDomain.DueChange.Count);
			Assert.Equal(0.50, MachineDomain.DueChange[0].Coin);
			Assert.Equal(0.10, MachineDomain.DueChange[1].Coin);
			Assert.Equal(0.10, MachineDomain.DueChange[2].Coin);
			Assert.Equal(9, MachineDomain.AvailableCoins.FindAll(c => c.Coin == 0.50).Count);
			Assert.Equal(8, MachineDomain.AvailableCoins.FindAll(c => c.Coin == 0.10).Count);

		}


		[Fact]
		public void GetChangeErrorPastelina1Test()
		{
			List<CoinDomain> ListCoins = new List<CoinDomain>();
			ListCoins.Add(new CoinDomain { Coin = 1.00 });

			List<ProductDomain> ListProduct = new List<ProductDomain>();
			ListProduct.Add(new ProductDomain { Name = "Pastelina", Quantity = 1 });

			DomainBase.provider.GetService<IInitializeMachineDomain>().Execute();
			DomainBase.provider.GetService<IInsertCoinsDomain>().Execute(ListCoins);
			Order(ListProduct, 9, 8, 10, 10);
			Order(ListProduct, 8, 6, 10, 10);
			Order(ListProduct, 7, 4, 10, 10);
			Order(ListProduct, 6, 2, 10, 10);
			Order(ListProduct, 5, 0, 10, 10);
			Order(ListProduct, 4, 0, 6, 10);
			Order(ListProduct, 3, 0, 2, 10);
			Order(ListProduct, 2, 0, 0, 0);

			DomainBase.provider.GetService<IRequestProductDomain>().Execute(ListProduct);
			var domain = DomainBase.provider.GetService<IGetChangeDomain>();
			Action act = () => domain.Execute();
			ValidationException ex = Assert.Throws<ValidationException>(act);
			Assert.Equal("NO_COINS. Sorry, I owe you..", ex.Message);

		}

		private void Order(List<ProductDomain> ListProduct, int qttExpect50, int qttExpect10, int qttExpect05, int qttExpect01)
		{
			DomainBase.provider.GetService<IRequestProductDomain>().Execute(ListProduct);
			DomainBase.provider.GetService<IGetChangeDomain>().Execute();
			Assert.Equal(qttExpect50, MachineDomain.AvailableCoins.FindAll(c => c.Coin == 0.50).Count);
			Assert.Equal(qttExpect10, MachineDomain.AvailableCoins.FindAll(c => c.Coin == 0.10).Count);
			Assert.Equal(qttExpect05, MachineDomain.AvailableCoins.FindAll(c => c.Coin == 0.05).Count);
			Assert.Equal(qttExpect01, MachineDomain.AvailableCoins.FindAll(c => c.Coin == 0.01).Count);

		}
	}
}
