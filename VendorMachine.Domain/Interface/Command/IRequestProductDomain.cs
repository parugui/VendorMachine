using System;
using System.Collections.Generic;
using System.Text;
using VendorMachine.Domain.Models;

namespace VendorMachine.Domain.Interface
{
	public interface IRequestProductDomain
	{
		public void Execute(List<ProductDomain> products);
	}
}
