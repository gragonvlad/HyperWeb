using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;

namespace HyperVWeb
{
	public static class PSHelper
	{
		public static string RunScript(string scriptText)
		{
			Runspace runspace = RunspaceFactory.CreateRunspace();
			runspace.Open();
			Pipeline pipeline = runspace.CreatePipeline();
			pipeline.Commands.AddScript(scriptText);
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