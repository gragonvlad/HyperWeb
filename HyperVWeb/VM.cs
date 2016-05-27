using HyperVWeb.Controllers;
using System;
using System.Runtime.CompilerServices;

namespace HyperVWeb
{
	public class VM
	{
		public DateTime CreationTime
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public VMEnabledState EnabledState
		{
			get;
			set;
		}

		public string EnabledStateString
		{
			get
			{
				return this.EnabledState.ToString("G");
			}
		}

		public Guid Id
		{
			get;
			set;
		}

		public DateTime InstallDate
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public TimeSpan Uptime
		{
			get;
			set;
		}

		public string UptimeString
		{
			get
			{
				return this.Uptime.ToString("g");
			}
		}

		public VM()
		{
		}
	}
}