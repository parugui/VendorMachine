using System;
using System.Collections.Generic;
using System.Text;

namespace VendorMachine.Application.ViewModels
{
	public class vmMachine
	{
		public List<vmCoin> InsertedCoins { get; set; }
		public List<vmCoin> AvailableCoins { get; set; }
		public List<vmCoin> DueChange { get; set; }
		public List<vmProduct> AvailableProducts { get; set; }
		public List<vmProduct> RequiredProducts { get; set; }
		public List<vmProductOutput> OutputProducts { get; set; }

		public double TotalInsertedCoins { get; set; }
		public double TotalRequiredProducts { get; set; }
	}
}
