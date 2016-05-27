using System;

namespace HyperVWeb.Controllers
{
	public class SaveController : GenericActionController
	{
		internal override string ScriptPattern
		{
			get
			{
				return "$vm = Get-VM -ComputerName {0} -Id {1}\r\nSave-VM $vm";
			}
		}

		public SaveController()
		{
		}
	}
}