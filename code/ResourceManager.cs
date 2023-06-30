using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Increlemental;

public class ResourceManager
{
	public Currency Coins { get; set; }

	public ResourceManager()
	{
		Coins = new Currency();
	}
}
