using System;
using System.Collections.Generic;
using System.Text;

namespace VendorMachine.Application.ViewModels
{
	public class vmProduct
	{
		public int ProductId { get; set; }
		public string Name { get; set; }

		public double Price { get; set; }
		public int Quantity { get; set; }
	}
}
