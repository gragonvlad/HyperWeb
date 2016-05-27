using System;

namespace HyperVWeb.Models
{
	public class VMSwitch
	{
		public Guid Id;

		public string Name;

		public string Notes;

		public string SwitchType;

		public string HyperVHost;

		public string NetAdapterInterfaceDescription;

		public VMSwitch()
		{
		}
	}
}