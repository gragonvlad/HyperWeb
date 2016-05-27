using System;

namespace HyperVWeb.Controllers
{
	public class StartController : GenericActionController
	{
		internal override string ScriptPattern
		{
			get
			{
				return "$vm = Get-VM -ComputerName {0} -Id {1}\r\nif ($vm.State -eq \"Paused\")\r\n{{\r\n    Resume-VM $vm\r\n}}\r\nelse\r\n{{\r\n    Start-VM $vm\r\n}}";
			}
		}

		public StartController()
		{
		}
	}
}