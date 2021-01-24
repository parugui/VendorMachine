using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using VendorMachine.Domain.Interface;
using VendorMachine.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Linq;

namespace VendorMachine.Domain.Test
{
	public class RequestProductDomainTest : DomainTestBase
	{
		[Theory]
		[ClassData(typeof(InsertSomeProducts))]
		public void InsertProductSucessTest(List<CoinDomain> coins, List<ProductDomain> products, int cntProductExpected)
		{
			DomainBase.provider.GetService<IInitializeMachineDomain>().Execute();
			DomainBase.provider.GetService<IInsertCoinsDomain>().Execute(coins);

			var domain = DomainBase.provider.GetService<IRequestProductDomain>();
			domain.Execute(products);
			Assert.Equal(cntProductExpected, MachineDomain.RequiredProducts.Count);
		}


		[Theory]
		[ClassData(typeof(InsertSomeProductsError))]
		public void InsertProductErrorTest(List<CoinDomain> coins, List<ProductDomain> products, string ErrorExpected)
		{
			DomainBase.provider.GetService<IInitializeMachineDomain>().Execute();
			DomainBase.provider.GetService<IInsertCoinsDomain>().Execute(coins);

			var domain = DomainBase.provider.GetService<IRequestProductDomain>();
			Action act = () => domain.Execute(products);
			ValidationException ex = Assert.Throws<ValidationException>(act);
			Assert.Equal(ErrorExpected, ex.Message);

		}

		[Fact]
		public void InsertProductSucessQttAvailableTest()
		{
			List<CoinDomain> ListCoins = new List<CoinDomain>();
			ListCoins.Add(new CoinDomain { Coin = 1.00 });
			ListCoins.Add(new CoinDomain { Coin = 0.50 });

			List<ProductDomain> ListProduct = new List<ProductDomain>();
			ListProduct.Add(new ProductDomain { Name = "Coke", Quantity = 1 });

			DomainBase.provider.GetService<IInitializeMachineDomain>().Execute();
			DomainBase.provider.GetService<IInsertCoinsDomain>().Execute(ListCoins);

			var domain = DomainBase.provider.GetService<IRequestProductDomain>();
			domain.Execute(ListProduct);
			Assert.Equal(9, MachineDomain.AvailableProducts.FirstOrDefault(p => p.Name == "Coke").Quantity);
			Assert.Single(MachineDomain.OutputProducts);
			Assert.Equal(0, MachineDomain.OutputProducts[0].Change);

		}

		[Fact]
		public void InsertProductSucessMultipleProducts()
		{
			List<CoinDomain> ListCoins = new List<CoinDomain>();
			ListCoins.Add(new CoinDomain { Coin = 1.00 });

			List<ProductDomain> ListProduct = new List<ProductDomain>();
			ListProduct.Add(new ProductDomain { Name = "Pastelina", Quantity = 3 });

			DomainBase.provider.GetService<IInitializeMachineDomain>().Execute();
			DomainBase.provider.GetService<IInsertCoinsDomain>().Execute(ListCoins);

			var domain = DomainBase.provider.GetService<IRequestProductDomain>();
			domain.Execute(ListProduct);
			Assert.Equal(7, MachineDomain.AvailableProducts.FirstOrDefault(p => p.Name == "Pastelina").Quantity);
			Assert.Equal(3, MachineDomain.OutputProducts.Count);
			Assert.Equal(0.70, MachineDomain.OutputProducts[0].Change);
			Assert.Equal(0.40, MachineDomain.OutputProducts[1].Change);
			Assert.Equal(0.10, MachineDomain.OutputProducts[2].Change);

		}

		public class InsertSomeProducts : IEnumerable<object[]>
		{
			public IEnumerator<object[]> GetEnumerator()
			{
				List<CoinDomain> ListCoins = new List<CoinDomain>();
				ListCoins.Add(new CoinDomain { Coin = 1.00 });
				ListCoins.Add(new CoinDomain { Coin = 1.00 });
				ListCoins.Add(new CoinDomain { Coin = 0.50 });

				List<ProductDomain> ListProduct = new List<ProductDomain>();
				ListProduct.Add(new ProductDomain { Name = "Coke", Quantity = 1 });
				ListProduct.Add(new ProductDomain { Name = "Water", Quantity = 1 });

				List<ProductDomain> ListProduct2 = new List<ProductDomain>();
				ListProduct2.Add(new ProductDomain { Name = "COKE", Quantity = 1 });
				ListProduct2.Add(new ProductDomain { Name = "WAter", Quantity = 1 });

				yield return new object[] { ListCoins, ListProduct, 2 };
				yield return new object[] { ListCoins, ListProduct2, 2 };


			}

			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		}

		public class InsertSomeProductsError : IEnumerable<object[]>
		{
			public IEnumerator<object[]> GetEnumerator()
			{
				List<CoinDomain> ListCoins = new List<CoinDomain>();
				ListCoins.Add(new CoinDomain { Coin = 1.00 });
				ListCoins.Add(new CoinDomain { Coin = 1.00 });
				ListCoins.Add(new CoinDomain { Coin = 0.50 });

				List<ProductDomain> ListProduct = new List<ProductDomain>();
				ListProduct.Add(new ProductDomain { Name = "Coke", Quantity = 1 });
				ListProduct.Add(new ProductDomain { Name = "Juice", Quantity = 1 });

				List<ProductDomain> ListProduct2 = new List<ProductDomain>();
				ListProduct2.Add(new ProductDomain { Name = "Coke", Quantity = 15 });

				List<ProductDomain> ListProduct3 = new List<ProductDomain>();
				ListProduct3.Add(new ProductDomain { Name = "Coke", Quantity = 3 });

				yield return new object[] { ListCoins, ListProduct, "NO_PRODUCT. Please request only Coke, Water, Pastelina, " };
				yield return new object[] { ListCoins, ListProduct2, "Unavailable Product. Insufficient Quantity. Sorry!" };
				yield return new object[] { new List<CoinDomain>(), ListProduct, "Please Insert some Coins.." };
				yield return new object[] { ListCoins, ListProduct3, "Insufficiente Coins to order the amount of product. Please Insert more Coins." };
			}

			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		}
	}
}
