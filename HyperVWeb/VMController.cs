using HyperVWeb.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Management;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Runtime.CompilerServices;
using System.Web.Http;
using Utils;
using WebAPI.OutputCache;
using WebAPI.OutputCache.TimeAttributes;

namespace HyperVWeb
{
	[AutoInvalidateCacheOutput]
	public class VMController : ApiController
	{
		internal static ConnectionOptions connectionOptions;

		internal static ConnectionOptions DefaultConnectionOptions
		{
			get
			{
				if (VMController.connectionOptions == null)
				{
					VMController.connectionOptions = new ConnectionOptions()
					{
						Authentication = AuthenticationLevel.PacketPrivacy,
						Impersonation = ImpersonationLevel.Impersonate
					};
				}
				return VMController.connectionOptions;
			}
		}

		public VMController()
		{
		}

		internal static int CompareString(string left, string right)
		{
			int num = 0;
			if (left == null && right == null)
			{
				return num;
			}
			if (left == null)
			{
				return -1;
			}
			if (right == null)
			{
				return 1;
			}
			return string.Compare(left, right, StringComparison.OrdinalIgnoreCase);
		}

		internal static int CompareVmByName(ManagementObject left, ManagementObject right)
		{
			return VMController.CompareWmiObject(left, right, "ElementName");
		}

		internal static int CompareWmiObject(ManagementObject left, ManagementObject right, string propertyName)
		{
			int num = 0;
			if (left == null)
			{
				if (right != null)
				{
					num = -1;
				}
				return num;
			}
			if (right == null)
			{
				return 1;
			}
			return string.Compare((string)left.GetPropertyValue(propertyName), (string)right.GetPropertyValue(propertyName), StringComparison.CurrentCultureIgnoreCase);
		}

		[CacheOutput(ClientTimeSpan=0, ServerTimeSpan=0, MustRevalidate=true)]
		public IEnumerable<dynamic> Get()
		{
			return VMController.wmi();
		}

		internal static Collection<PSObject> GetAllVM(string computerName)
		{
            //VMController.GetManagementScopeForComputer(computerName);
            //return VMController.GetVMByQuery(computerName, "select * from Msvm_ComputerSystem");
            Runspace runspace = RunspaceFactory.CreateRunspace();
            runspace.Open();
            Pipeline pipeline = runspace.CreatePipeline();
            pipeline.Commands.AddScript("Get-VM");
            Collection<PSObject> pSObjects = pipeline.Invoke();
            runspace.Close();
		    return pSObjects;
		}

		internal static ManagementScope GetManagementScopeForComputer(string computerName)
		{
			if (string.IsNullOrEmpty(computerName))
			{
				return new ManagementScope("root\\virtualization\\v2", VMController.DefaultConnectionOptions);
			}
			CultureInfo installedUICulture = CultureInfo.InstalledUICulture;
			object[] objArray = new object[] { computerName };
			return new ManagementScope(string.Format(installedUICulture, "\\\\{0}\\root\\virtualization\\v2", objArray), VMController.DefaultConnectionOptions);
		}

		internal static string GetNetBiosNameFromFullName(string fullName)
		{
			string str = fullName;
			if (string.IsNullOrEmpty(fullName))
			{
				return str;
			}
			int num = fullName.IndexOf('.');
			if (num != -1)
			{
				str = fullName.Substring(0, num);
			}
			if (str.Length > 15)
			{
				str = str.Substring(0, 15);
			}
			return str.ToUpperInvariant();
		}

		[CacheOutputUntil(2000, 1, 1, 1, 1, 1)]
		public static List<ManagementObject> GetVMByQuery(string computerName, string query)
		{
			List<ManagementObject> managementObjects;
			ObjectQuery objectQuery = new ObjectQuery(query);
			ManagementScope managementScopeForComputer = VMController.GetManagementScopeForComputer(computerName);
			List<ManagementObject> managementObjects1 = new List<ManagementObject>();
			using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(managementScopeForComputer, objectQuery))
			{
				foreach (ManagementObject managementObject in managementObjectSearcher.Get())
				{
					if (managementObject == null)
					{
						continue;
					}
					string item = (string)managementObject["Name"];
					string netBiosNameFromFullName = VMController.GetNetBiosNameFromFullName(computerName);
					if (VMController.CompareString(computerName, item) == 0 || VMController.CompareString(netBiosNameFromFullName, item) == 0)
					{
						continue;
					}
					managementObjects1.Add(managementObject);
				}
				managementObjects1.Sort(new Comparison<ManagementObject>(VMController.CompareVmByName));
				managementObjects = managementObjects1;
			}
			return managementObjects;
		}

		private static DateTime parsedate(string date)
		{
			string str = date.Substring(0, "yyyyMMddHHmmss.ffffff".Length);
			return DateTime.ParseExact(str, "yyyyMMddHHmmss.ffffff", CultureInfo.CurrentCulture);
		}

		private static IEnumerable<dynamic> wmi()
		{
			Guid guid;
			string machineName = Environment.MachineName;
			machineName = "localhost";
			foreach (PSObject allVM in VMController.GetAllVM(machineName))
			{
				//VM vM = new VM()
				//{
				//	Name = allVM.SystemProperties["ElementName"].Value as string
				//};
				//string str = allVM.SystemProperties["Name"].Value.ToString();
				//if (!Guid.TryParse(str, out guid))
				//{
				//	continue;
				//}
				//vM.Id = guid;
				//string value = (string)allVM.SystemProperties["InstallDate"].Value;
				//vM.CreationTime = VMController.parsedate(value);
				//vM.Description = allVM.SystemProperties["Description"].Value as string;
				//vM.EnabledState = (VMEnabledState)allVM.SystemProperties["EnabledState"].Value;
				//ulong num = (ulong)allVM.SystemProperties["OnTimeInMilliseconds"].Value;
				//vM.Uptime = TimeSpan.FromMilliseconds((double)((float)num));
				//yield return vM;
			    var a = allVM.ToDynamic();

                yield return a;
			}
		}

	}
}