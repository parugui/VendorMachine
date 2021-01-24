using System;
using System.Collections.Generic;
using System.Text;

namespace VendorMachine.Domain.Models
{
	public static class MachineDomain
	{
		public static List<CoinDomain> InsertedCoins { get; set; }
		public static List<CoinDomain> AvailableCoins { get; set; }
		public static List<CoinDomain> DueChange { get; set; }
		public static List<ProductDomain> AvailableProducts { get; set; }
		public static List<ProductDomain> RequiredProducts { get; set; }
		public static List<ProductOutputDomain> OutputProducts { get; set; }




	}
}
