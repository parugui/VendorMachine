using System;
using System.Collections.Generic;
using System.Text;
using VendorMachine.Application.ViewModels;

namespace VendorMachine.Application.Interfaces
{
	public interface IMachine
	{
		public void Initialize();
		public void InsertCoins(List<vmCoin> ListCoins);

		public vmMachine RequestProduct(List<vmProduct> ListProduct);

		public vmMachine GetChange();


	}
}
