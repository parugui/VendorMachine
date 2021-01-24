using System;
using System.Collections.Generic;
using System.Text;
using VendorMachine.Domain.Interface;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using VendorMachine.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace VendorMachine.Domain.Test
{
	public class InsertCoinsDomainTest : DomainTestBase
	{

		[Theory]
		[ClassData(typeof(InsertSomeCoins))]
		public void InsertCoinsSucessTest(List<CoinDomain> ListCoins, int qttExpected, int qttAvailabeExpected)
		{
			DomainBase.provider.GetService<IInitializeMachineDomain>().Execute();
			DomainBase.provider.GetService<IInsertCoinsDomain>().Execute(ListCoins);
			Assert.Equal(qttExpected, MachineDomain.InsertedCoins.Count);
			Assert.Equal(qttAvailabeExpected, MachineDomain.AvailableCoins.Count);

		}

		[Fact]
		public void InsertCoinsErrorTest()
		{
			DomainBase.provider.GetService<IInitializeMachineDomain>().Execute();
			List<CoinDomain> coins = new List<CoinDomain>();
			coins.Add(new CoinDomain { Coin = 0.05 });
			coins.Add(new CoinDomain { Coin = 0.01 });
			coins.Add(new CoinDomain { Coin = 0.00 });

			var domain = DomainBase.provider.GetService<IInsertCoinsDomain>();
			Action act = () => domain.Execute(coins);
			ValidationException ex = Assert.Throws<ValidationException>(act);
			Assert.Equal("Coin not Accepted: 0.00. Please Insert only 0.01, 0.05, 0.10, 0.25, 0.50, 1.00", ex.Message);
		}

		public class InsertSomeCoins : IEnumerable<object[]>
		{

			public IEnumerator<object[]> GetEnumerator()
			{
				List<CoinDomain> ListCoins = new List<CoinDomain>();
				ListCoins.Add(new CoinDomain { Coin = 0.05 });
				ListCoins.Add(new CoinDomain { Coin = 0.01 });

				List<CoinDomain> ListCoins1 = new List<CoinDomain>();

				yield return new object[] { ListCoins, 2, 62 };
				yield return new object[] { ListCoins1, 0, 60 };
			}

			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();



		}
	}
}
