using System;

namespace HyperVWeb.Controllers
{
	public class StopController : GenericActionController
	{
		internal override string ScriptPattern
		{
			get
			{
				return "$vm = Get-VM -ComputerName {0} -Id {1}\r\nStop-VM $vm –TurnOff";
			}
		}

		public StopController()
		{
		}
	}
}