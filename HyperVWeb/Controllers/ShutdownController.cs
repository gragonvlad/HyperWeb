using System;

namespace HyperVWeb.Controllers
{
	public class ShutdownController : GenericActionController
	{
		internal override string ScriptPattern
		{
			get
			{
				return "$vm = Get-VM -ComputerName {0} -Id {1}\r\nStop-VM $vm";
			}
		}

		public ShutdownController()
		{
		}
	}
}