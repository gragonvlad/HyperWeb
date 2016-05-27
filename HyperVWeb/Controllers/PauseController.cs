using System;

namespace HyperVWeb.Controllers
{
	public class PauseController : GenericActionController
	{
		internal override string ScriptPattern
		{
			get
			{
				return "$vm = Get-VM -ComputerName {0} -Id {1}\r\nSuspend-VM $vm";
			}
		}

		public PauseController()
		{
		}
	}
}