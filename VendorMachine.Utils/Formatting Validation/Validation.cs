using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;


namespace VendorMachine.Utils
{
	public class Validation
	{
		public static bool isDouble(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return false;
			}
			else
			{
				double d;
				if (!Double.TryParse(value, out d))
				{
					return false;
				}
			}

			return true;
		}

	}
}
