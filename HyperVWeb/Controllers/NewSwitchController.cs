using HyperVWeb;
using HyperVWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using WebAPI.OutputCache;

namespace HyperVWeb.Controllers
{
	public class NewSwitchController : SwitchController
	{
		public NewSwitchController()
		{
		}

		[CacheOutput(ClientTimeSpan=0, ServerTimeSpan=0, MustRevalidate=true)]
		public IEnumerable<VMSwitch> Post(VMSwitch sw)
		{
			string empty = string.Empty;
			string[] strArrays = new string[] { "Private", "Internal" };
			if (strArrays.Contains<string>(sw.SwitchType))
			{
				string str = "New-VMSwitch -Name {1} -ComputerName {0} -Notes {3} -SwitchType {4}";
				object[] hyperVHost = new object[] { sw.HyperVHost, sw.Name, sw.Notes, sw.SwitchType };
				empty = string.Format(str, hyperVHost);
			}
			else if (!string.IsNullOrWhiteSpace(sw.NetAdapterInterfaceDescription))
			{
				string str1 = "New-VMSwitch -Name {1} -ComputerName {0} -Notes {3} -InterfaceDecription {4}";
				object[] objArray = new object[] { sw.HyperVHost, sw.Name, sw.Notes, sw.NetAdapterInterfaceDescription };
				empty = string.Format(str1, objArray);
			}
			if (!string.IsNullOrWhiteSpace(empty))
			{
				PSHelper.RunScript(empty);
			}
			return base.Get();
		}
	}
}