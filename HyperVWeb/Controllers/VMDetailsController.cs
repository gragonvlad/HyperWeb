using HyperVWeb.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Web.Http;
using WebAPI.OutputCache;

namespace HyperVWeb.Controllers
{
	public class VMDetailsController : ApiController
	{
		public VMDetailsController()
		{
		}

		[CacheOutput(ClientTimeSpan=0, ServerTimeSpan=0, MustRevalidate=true)]
		public object Get()
		{
			return new CreateVM();
		}

		private string RunScript(string scriptText)
		{
			Runspace runspace = RunspaceFactory.CreateRunspace();
			runspace.Open();
			Pipeline pipeline = runspace.CreatePipeline();
			pipeline.Commands.AddScript(string.Concat(scriptText, " | ConvertTo-JSON -Depth 50"));
			Collection<PSObject> pSObjects = pipeline.Invoke();
			runspace.Close();
			StringBuilder stringBuilder = new StringBuilder();
			foreach (PSObject pSObject in pSObjects)
			{
				stringBuilder.AppendLine(pSObject.ToString());
			}
			return stringBuilder.ToString();
		}
	}
}