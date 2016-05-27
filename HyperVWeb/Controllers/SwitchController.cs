using HyperVWeb.Models;
using Microsoft.HyperV.PowerShell;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Runtime.CompilerServices;
using System.Web.Http;
using WebAPI.OutputCache;

namespace HyperVWeb.Controllers
{
	public class SwitchController : ApiController
	{
		private string HyperVHost = "localhost";

		public SwitchController()
		{
		}

		[CacheOutput(ClientTimeSpan=0, ServerTimeSpan=0, MustRevalidate=true)]
		public IEnumerable<HyperVWeb.Models.VMSwitch> Get()
		{
			return this.RunScript(string.Format("Get-VMSwitch -ComputerName {0}", this.HyperVHost));
		}

		private IEnumerable<HyperVWeb.Models.VMSwitch> RunScript(string scriptText)
		{
			Runspace runspace = RunspaceFactory.CreateRunspace();
			runspace.Open();
			Pipeline pipeline = runspace.CreatePipeline();
			pipeline.Commands.AddScript(scriptText);
			Collection<PSObject> pSObjects = pipeline.Invoke();
			runspace.Close();
			foreach (PSObject pSObject in pSObjects)
			{
				HyperVWeb.Models.VMSwitch vMSwitch = new HyperVWeb.Models.VMSwitch();
				Microsoft.HyperV.PowerShell.VMSwitch immediateBaseObject = (Microsoft.HyperV.PowerShell.VMSwitch)pSObject.ImmediateBaseObject;
				vMSwitch.Name = immediateBaseObject.Name;
				vMSwitch.Id = immediateBaseObject.Id;
				vMSwitch.Notes = immediateBaseObject.Notes;
				vMSwitch.SwitchType = immediateBaseObject.SwitchType.ToString("G");
				vMSwitch.HyperVHost = this.HyperVHost;
				vMSwitch.NetAdapterInterfaceDescription = immediateBaseObject.NetAdapterInterfaceDescription;
				yield return vMSwitch;
			}
		}
	}
}