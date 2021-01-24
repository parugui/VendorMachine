using System;
using System.Collections.Generic;
using System.Text;

namespace VendorMachine.Domain.Models
{
	public class ProductOutputDomain : ProductDomain
	{
		public double Change { get; set; }
	}
}
