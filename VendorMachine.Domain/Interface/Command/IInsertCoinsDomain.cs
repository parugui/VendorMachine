using System;
using System.Collections.Generic;
using System.Text;
using VendorMachine.Domain.Models;

namespace VendorMachine.Domain.Interface
{
	public interface IInsertCoinsDomain
	{
		public void Execute(List<CoinDomain> coins);
	}
}
