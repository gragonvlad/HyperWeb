using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Topshelf;
using Topshelf.HostConfigurators;
using Topshelf.Runtime;
using Topshelf.ServiceConfigurators;

namespace HyperVWeb
{
	internal class Program
	{
		public Program()
		{
		}

		private static void Main(string[] args)
		{
			HostFactory.Run((HostConfigurator x) => {
				x.UseNLog();
				x.Service<WindowsService>((ServiceConfigurator<WindowsService> s) => {
					s.ConstructUsing((HostSettings name) => new WindowsService());
					s.WhenStarted<WindowsService>((WindowsService svc) => svc.Start());
					s.WhenStopped<WindowsService>((WindowsService svc) => svc.Stop());
				});
				x.RunAsLocalSystem();
				FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
				x.SetDescription(versionInfo.FileDescription);
				x.SetDisplayName(versionInfo.ProductName);
				x.SetServiceName(versionInfo.InternalName);
			});
		}
	}
}