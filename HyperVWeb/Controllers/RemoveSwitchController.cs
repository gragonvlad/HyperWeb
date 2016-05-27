using HyperVWeb;
using HyperVWeb.Models;
using System;
using System.Collections.Generic;

namespace HyperVWeb.Controllers
{
	internal class RemoveSwitchController : SwitchController
	{
		public RemoveSwitchController()
		{
		}

		public IEnumerable<VMSwitch> Post(VMSwitch sw)
		{
			string str = string.Format("$sw = Get-VMSwitch -Id {0}\r\nRemove-VMSwitch $sw -Force", sw.Id);
			PSHelper.RunScript(str);
			return base.Get();
		}
	}
}